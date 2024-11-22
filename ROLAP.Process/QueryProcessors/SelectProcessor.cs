using ROLAP.Core.Models.Helpers;
using ROLAP.Core.Models.Interfaces.CubeItem;
using ROLAP.Core.Models.Model.Query;
using ROLAP.Loaders.Helpers;
using ROLAP.Process.Models.Result;

namespace ROLAP.Process.QueryProcessors;

internal class SelectProcessor
{
    internal CubeResult ExecuteQuery(CubeQuery query)
    {
        var values = ValuesHelper.LoadValues(query.Sets, query.Where);
        
        var sets = PrepareSets(query.Sets);
        var aggregatedValues = FillSetsValues(values, sets);
        return new CubeResult(sets,aggregatedValues);
    }
    
    #region Aggregate
    
    private List<CubeItemSet> PrepareSets(IEnumerable<CubeItemSet> sets, bool isAggregate = true)
    {
        List<CubeItemSet> resSets = new List<CubeItemSet>();
    
        foreach (var set in sets)
        {
            List<CubeItemTuple> resultTuples = new List<CubeItemTuple>();
            var members = set.Tuples.ToList();
            if (members.Any())
            {
                if (isAggregate)
                {
                    resultTuples = GetTuples(members[^1], isAggregate);
                    for (int i = members.Count - 2; i >= 0; i--)
                    {
                        resultTuples = Merge(GetTuples(members[i], isAggregate), resultTuples);
                    }
                }
                else
                {
                    foreach (var member in members)
                    {
                        resultTuples.AddRange(GetTuples(member, isAggregate));
                    }
                }
            }
    
            resSets.Add(new CubeItemSet(resultTuples));
            
        }
    
        return resSets;
    }
    
    private List<CubeItemTuple> Merge(IEnumerable<CubeItemTuple> tuples1, IEnumerable<CubeItemTuple> tuples2)
    {
        if (tuples1 is null && tuples2 is not null) return tuples2.ToList();
        if (tuples2 is null && tuples1 is not null) return tuples1.ToList();
        if (tuples2 is null && tuples1 is null) return new List<CubeItemTuple>();
        List<CubeItemTuple> res = new List<CubeItemTuple>();
        
        foreach (var tuple1 in tuples1)
        {
            foreach (var tuple2 in tuples2)
            {
                List<ICubeItem> items = new List<ICubeItem>();
                foreach (var tupleMember in tuple1.Members)
                {
                    items.Add(tupleMember.Clone(true));
                }
                
                foreach (var tupleMember in tuple2.Members)
                {
                    items.Add(tupleMember.Clone(true));
                }
                var newTuple = new CubeItemTuple(items);
                
                res.Add(newTuple);
            }
        }
        
        return res;
    }
    
    private List<CubeItemTuple> GetTuples(CubeItemTuple item, bool isAggregate)
    {
        ICubeItem fistItem = item.Members.First();
        
        if (fistItem is IMeasureCubeItem) return GetMeasureTuples(item.Members.Cast<IMeasureCubeItem>(),isAggregate);
        if (fistItem is IDimensionCubeItem) return GetDimensionTuples(item.Members.Cast<IDimensionCubeItem>(),isAggregate);
        throw new Exception("asdasd");
    }
    
    private List<CubeItemTuple> GetMeasureTuples(IEnumerable<IMeasureCubeItem> measures, bool isAggregate)
    {
        List<CubeItemTuple> res = new List<CubeItemTuple>();
    
        // if (isAggregate)
        // {
        //     res.Add(new CubeItemTuple(new List<ICubeItem>
        //     {
        //         measures.First().GetTotalItem()
        //     }));
        // }
    
        foreach (var measure in measures)
        {
            res.Add( new CubeItemTuple(new List<ICubeItem>
            {
                measure.Clone(false)
            }));
        }
    
        return res;
    }
    
    private List<CubeItemTuple> GetDimensionTuples(IEnumerable<IDimensionCubeItem> dimensions, bool isAggregate)
    {
        List<CubeItemTuple> res = new List<CubeItemTuple>();
    
        foreach (var dimension in dimensions)
        {
            if (isAggregate)
            {
                res.Add(new CubeItemTuple(new List<ICubeItem>
                {
                    dimension.Clone(false)
                }));
            }

            foreach (var dimensionValue in dimension.GetDimensions())
            {
                var clone = (IDimensionCubeItem)dimension.Clone(false);
                clone.AddDimension((IDimensionCubeItem)dimensionValue.Clone(false));
                
                res.Add(new CubeItemTuple(new List<ICubeItem>
                {
                    clone
                }));
            }
        }
        
        // foreach (var member in item.Members.Cast<IDimensionCubeItem>())
        // {
        //     IDimensionCubeItem clone = item
        //     member
        // }
    
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

        if (!values.Any()) return resValues;
        var valuesMeasure = values.First().GetMeasure();
        
        foreach (var tuple in tuples)
        {
            List<IValueCubeItem> tupleValues = new List<IValueCubeItem>();
            foreach (var value in values)
            {
                if(CubeItemHelper.IsValueInTuple(value,tuple)) 
                    tupleValues.Add(value);
            }
            
            resValues.Add(valuesMeasure.Aggregate(tupleValues));
        }
    
        return resValues;
    }
    
     #endregion
}