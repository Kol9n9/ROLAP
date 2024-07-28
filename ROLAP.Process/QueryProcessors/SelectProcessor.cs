using ROLAP.Core.Models.Interfaces.CubeItem;
using ROLAP.Core.Models.Model.Query;
using ROLAP.Loaders.Helpers;
using ROLAP.Process.Models.Result;
using ROLAP.Core.Models.Interfaces.Container;

namespace ROLAP.Process.QueryProcessors;

internal class SelectProcessor
{
    internal CubeResult ExecuteQuery(CubeQuery query)
    {
        IEnumerable<CubeItemTuple> setItems = query.Sets;
        var values = ValuesHelper.LoadValues(setItems, query.Where);

        var sets = ConvertTupleItems(setItems);
        var aggregatedValues = FillSetsValues(values, sets);
        return new CubeResult(sets,aggregatedValues);
    }

    #region Aggregate
    
    private List<CubeItemSet> ConvertTupleItems(IEnumerable<CubeItemTuple> tupleItems, bool isAggregate = true)
    {
        List<CubeItemSet> sets = new List<CubeItemSet>();
    
        foreach (var tuple in tupleItems)
        {
            List<CubeItemTuple> resultTuples = new List<CubeItemTuple>();
            var members = tuple.Members.ToList();
            if (members.Any())
            {
                if (isAggregate)
                {
                    resultTuples = GetTuples(members[^1]);
                    for (int i = members.Count - 2; i >= 0; i--)
                    {
                        resultTuples = Merge(GetTuples(members[i]), resultTuples);
                    }
                }
                else
                {
                    foreach (var member in members)
                    {
                        resultTuples.AddRange(GetTuples(member,isAggregate));
                    }
                }
            }
            sets.Add(new CubeItemSet(resultTuples));
        }
    
        return sets;
    }
    
    private List<CubeItemTuple> Merge(IEnumerable<CubeItemTuple> tuples1, IEnumerable<CubeItemTuple> tuples2)
    {
        List<CubeItemTuple> res = new List<CubeItemTuple>();
        
        foreach (var tuple1 in tuples1)
        {
            foreach (var tuple2 in tuples2)
            {
                List<IContainer> items = new List<IContainer>();
                foreach (var tupleMember in tuple1.Members)
                {
                    items.Add(tupleMember.Clone<IContainer>(true));
                }
                
                foreach (var tupleMember in tuple2.Members)
                {
                    items.Add(tupleMember.Clone<IContainer>(true));
                }
                var newTuple = new CubeItemTuple(items);
                
                res.Add(newTuple);
            }
        }
        
        return res;
    }
    
    private List<CubeItemTuple> GetTuples(ICubeItem item, bool isAggregate = true)
    {
        if (item is not IContainer container) throw new InvalidCastException();
        
        List<CubeItemTuple> res = new List<CubeItemTuple>();
        if (isAggregate)
        {
            res.Add(new CubeItemTuple(new List<IContainer>{container.Clone<IContainer>(false)}));
        }

        foreach (var value in container.GetValues())
        {
            IContainer copy = container.Clone<IContainer>(false);
            copy.AddValue(value.Clone<ICubeItem>(false));
            res.Add(new CubeItemTuple(new List<IContainer>{copy}));
        }

        return res;
    }
    
    private IEnumerable<ICubeItem> FillSetsValues(IEnumerable<IValueCubeItem> values, IEnumerable<CubeItemSet> sets, IEnumerable<CubeItemTuple> prevTuples = null)
    {
        List<ICubeItem> resValues = new List<ICubeItem>();
    
        if (sets.Count() == 1)
        {
            List<CubeItemTuple> tuples = Merge(prevTuples,sets.FirstOrDefault().Tuples);
            resValues = FillTupleValues(values, tuples).ToList();
        }
        else
        {
            var lastSet = sets.LastOrDefault();
            
            foreach (var tuple in lastSet.Tuples)
            {
                var copyTuples = new List<CubeItemTuple> { tuple };
                if (prevTuples is not null)
                {
                    copyTuples = Merge(prevTuples, copyTuples);
                }
                resValues.AddRange(FillSetsValues(values,sets.Take(sets.Count()-1),copyTuples));
            }
        }
    
        return resValues;
    }
    
    private IEnumerable<ICubeItem> FillTupleValues(IEnumerable<IValueCubeItem> values,
        IEnumerable<CubeItemTuple> tuples)
    {
        List<ICubeItem> resValues = new List<ICubeItem>();
        
        foreach (var tuple in tuples)
        {
            List<IValueCubeItem> tupleValues = new List<IValueCubeItem>();
            foreach (var value in values)
            {
                if(CubeItemHelper.IsCubeItemInContainers(value,tuple.Members)) tupleValues.Add(value);
            }
            resValues.Add(ValuesHelper.AggregatedValues(tupleValues));
        }
    
        return resValues;
    }
    
     #endregion
}