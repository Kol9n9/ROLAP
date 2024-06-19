// using ROLAP.Core.Models.Helpers;
// using ROLAP.Core.Models.Interfaces;
// using ROLAP.Core.Models.Model.CubeItem;
// using ROLAP.Core.Models.Model.Query;
// using ROLAP.Loaders.Helpers;
// using ROLAP.Process.Models.Result;
//
// namespace ROLAP.Process.QueryProcessors;
//
// internal class SelectProcessor
// {
//     internal CubeResult ExecuteQuery(CubeQuery query)
//     {
//         IEnumerable<CubeItemTuple> setItems = query.Sets;
//         var values = LoadValues(setItems, query.Where);
//
//         var sets = ConvertTupleItems(setItems);
//         var aggregatedValues = FillSetsValues(values, sets);
//         CubeResult res = new CubeResult(sets,aggregatedValues);
//         return res;
//     }
//     
//     #region LoadValues
//
//     private IEnumerable<ValueCubeItem> LoadValues(IEnumerable<CubeItemTuple> tupleItems, IEnumerable<CubeItemTuple> whereTupleItems)
//     {
//         List<ValueCubeItem> values = new List<ValueCubeItem>();
//
//         var measures =
//             tupleItems.Select(x => x.Members.Where(x => x is MeasureGroupCubeItem)).SelectMany(x => x.SelectMany(y => y.GetValues())).Cast<MeasureCubeItem>().ToList();
//
//         var dimensions =
//             tupleItems.Select(x => x.Members.Where(x => x is DimensionCubeItem)).SelectMany(x => x).Cast<DimensionCubeItem>().ToList();
//
//         measures.AddRange(whereTupleItems.Select(x => x.Members.Where(x => x is MeasureGroupCubeItem)).SelectMany(x => x.SelectMany(y => y.GetValues())).Cast<MeasureCubeItem>().ToList());
//         dimensions.AddRange(whereTupleItems.Select(x => x.Members.Where(x => x is DimensionCubeItem)).SelectMany(x => x).Cast<DimensionCubeItem>().ToList());
//
//         ILoadOptions optionsList = LoadOptionsHelper.GetValueOptionsByDimension(dimensions);
//
//         foreach (var measure in measures)
//         {
//             values.AddRange(measure.GetLoader().Load(new List<ILoadOptions>{optionsList}));
//         }
//        
//         return values;
//     }
//     
//     #endregion
//
//     #region Aggregate
//
//     private List<CubeResultSet> ConvertTupleItems(IEnumerable<CubeItemTuple> tupleItems, bool isAggregate = true)
//     {
//         List<CubeResultSet> sets = new List<CubeResultSet>();
//
//         foreach (var tuple in tupleItems)
//         {
//             List<CubeResultTuple> resultTuples = new List<CubeResultTuple>();
//             var members = tuple.Members.ToList();
//             if (members.Any())
//             {
//                 if (isAggregate)
//                 {
//                     resultTuples = GetTuples(members[^1]);
//                     for (int i = members.Count - 2; i >= 0; i--)
//                     {
//                         resultTuples = Merge(GetTuples(members[i]), resultTuples);
//                     }
//                 }
//                 else
//                 {
//                     foreach (var member in members)
//                     {
//                         resultTuples.AddRange(GetTuples(member,isAggregate));
//                     }
//                 }
//             }
//             sets.Add(new CubeResultSet(resultTuples));
//         }
//
//         return sets;
//     }
//     
//     
//     private List<CubeResultTuple> Merge(IEnumerable<CubeResultTuple> tuples1, IEnumerable<CubeResultTuple> tuples2)
//     {
//         List<CubeResultTuple> res = new List<CubeResultTuple>();
//
//         foreach (var tuple1 in tuples1)
//         {
//             foreach (var tuple2 in tuples2)
//             {
//                 List<ICubeItem> items = new List<ICubeItem>();
//                 foreach (var tupleMember in tuple1.Members)
//                 {
//                     items.Add(tupleMember.Clone());
//                 }
//
//                 foreach (var tupleMember in tuple2.Members)
//                 {
//                     items.Add(tupleMember.Clone());
//                 }
//                 var newTuple = new CubeResultTuple(items);
//                 res.Add(newTuple);
//             }
//         }
//     
//         return res;
//     }
//     
//     private List<CubeResultTuple> GetTuples(ICubeItem cubeItem, bool isAggregate = true)
//     {
//         List<CubeResultTuple> res = new List<CubeResultTuple>();
//
//         if (isAggregate)
//         {
//             res.Add(new CubeResultTuple(new List<ICubeItem>{cubeItem.Clone(false)}));
//         }
//
//         foreach (var value in cubeItem.GetValues())
//         {
//             var copy = cubeItem.Clone(false);
//             copy.AddValue(value.Clone(false));
//             res.Add(new CubeResultTuple(new List<ICubeItem>{copy}));
//         }
//         return res;
//     }
//
//     private IEnumerable<ValueCubeItem> FillSetsValues(IEnumerable<ValueCubeItem> values, IEnumerable<CubeResultSet> sets, IEnumerable<CubeResultTuple> prevTuples = null)
//     {
//         List<ValueCubeItem> resValues = new List<ValueCubeItem>();
//
//         if (sets.Count() == 1)
//         {
//             List<CubeResultTuple> tuples = Merge(prevTuples,sets.FirstOrDefault().Tuples);
//             resValues = FillTupleValues(values, tuples).ToList();
//         }
//         else
//         {
//             var lastSet = sets.LastOrDefault();
//             List<CubeResultTuple> tuples = new List<CubeResultTuple>();
//             foreach (var tuple in lastSet.Tuples)
//             {
//                 var copyTuples = new List<CubeResultTuple> { tuple };
//                 if (prevTuples is not null)
//                 {
//                     copyTuples = Merge(prevTuples, copyTuples);
//                 }
//                 resValues.AddRange(FillSetsValues(values,sets.Take(sets.Count()-1),copyTuples));
//             }
//         }
//
//         return resValues;
//     }
//
//     private IEnumerable<ValueCubeItem> FillTupleValues(IEnumerable<ValueCubeItem> values,
//         IEnumerable<CubeResultTuple> tuples)
//     {
//         List<ValueCubeItem> resValues = new List<ValueCubeItem>();
//         
//         foreach (var tuple in tuples)
//         {
//             List<ValueCubeItem> tupleValues = new List<ValueCubeItem>();
//             foreach (var value in values)
//             {
//                 if(IsValueAggregatated(value,tuple.Members)) tupleValues.Add(value);
//             }
//             resValues.Add(AggregatedValues(tupleValues));
//         }
//
//         return resValues;
//     }
//
//     private bool IsValueAggregatated(ValueCubeItem value, IEnumerable<ICubeItem> members)
//     {
//         var measure = (MeasureCubeItem)members.FirstOrDefault(x => x is MeasureGroupCubeItem)?.GetValues()
//             .FirstOrDefault();
//         if (measure is not null && value.Measure.NameEqual(measure.Key)) return false;
//         var dimensions = members.Where(x => x is DimensionCubeItem).Cast<DimensionCubeItem>();
//         return CubeItemHelper.IsValueInDimensions(value, dimensions);
//     }
//
//     private ValueCubeItem AggregatedValues(IEnumerable<ValueCubeItem> values)
//     {
//         var first = values.FirstOrDefault();
//         return first ?? new ValueCubeItem("", "-", null, null);
//     }
//     
//     #endregion
// }