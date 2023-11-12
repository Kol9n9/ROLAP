using ROLAP.Common.Model.Interfaces;
using ROLAP.Common.Model.Models;
using ROLAP.Process.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ROLAP.Common.Model.Models.Meta;
using ROLAP.Values;

namespace ROLAP.Process
{
    internal class CubeProcessSelect
    {
        private readonly IRepository _repository;
        private readonly CubeQuery _cubeQuery;
        private readonly List<CubeMeasureMeta> _cubeMeasureMeta;
        private readonly List<CubeDimensionMeta> _cubeDimensionMeta;
        public CubeProcessSelect(IRepository repository, CubeQuery cubeQuery, List<ICubeMeta> cubeMeta) {
            _repository = repository;
            _cubeQuery = cubeQuery;
            _cubeMeasureMeta = cubeMeta.Where(x => x is CubeMeasureMeta).Cast<CubeMeasureMeta>().ToList();
            _cubeDimensionMeta = cubeMeta.Where(x => x is CubeDimensionMeta).Cast<CubeDimensionMeta>().ToList();
        }
        public Cube Process()
        {
            Cube cube = new Cube();
            cube.Axes = GenerateAxes(_cubeQuery);
            var values = GetValues(_cubeQuery);
            //cube.Values = SetAxesValues(_cubeQuery.Axes, values);
            return null;
        }
        private List<CubeAxis> GenerateAxes(CubeQuery request)
        {
            List<CubeAxis> axes = new List<CubeAxis>();
            foreach (var requestAxis in request.Axes)
            {
                //axes.Add(GenerateAxis(requestAxis));
            }
            return axes;
        }
        // private CubeAxis GenerateAxis(CubeQueryAxis axis)
        // {
        //     CubeAxis resultAxis = new CubeAxis();
        //    
        //     foreach (var tuple in axis.Set.Tuples)
        //     {
        //         var cubeMetaItems = GetCubeDimension(tuple);
        //         CubeTuple newTuple = new CubeTuple();
        //         foreach (var cubeMetaItem in cubeMetaItems)
        //         {
        //             newTuple.Members.Add(new CubeMember
        //             {
        //                 Name = cubeMetaItem.Name
        //             });
        //         }
        //         resultAxis.Tuples.Add(newTuple);
        //     }
        //     
        //     return resultAxis;
        // }
        
        private List<CubeQueryValue> GetValues(CubeQuery cubeAxes)
        {
            List<ICubeMeta> meta = new List<ICubeMeta>();
            foreach (var axis in cubeAxes.Axes) 
            {
                foreach (var tuple in axis.Set.Tuples)
                {
                    foreach (var member in tuple.Members)
                    {
                        switch (member.Type)
                        {
                            // case CubeMemberType.Dimension:
                            // {
                            //     var dimension = GetMetaDimensionValue(member);
                            //     if(dimension == null) continue;
                            //     meta.Add(dimension);
                            //     break;
                            // }
                            // case CubeMemberType.Measure:
                            // {
                            //     var measure = GetMetaMeasure(member);
                            //     if(measure == null) continue;
                            //     meta.Add(measure);
                            //     break;
                            // }
                        }
                    }
                }
            }

            return CubeValues.GetValues(_repository, meta);
        }
        // private List<CubeValue> SetAxesValues(List<CubeQueryAxis> cubeAxis, List<CubeQueryValue> values, List<CubeQueryTuple> prevAxisTuples = null)
        // {
        //     if (cubeAxis.Count == 1)
        //     {
        //         return SetAxisValues(cubeAxis[0].Set.Tuples, values, prevAxisTuples);
        //     }
        //     List<CubeValue> resultValues = new List<CubeValue>();
        //     List<CubeQueryTuple> prevTuples = new List<CubeQueryTuple>();
        //     if (prevAxisTuples != null)
        //     {
        //         prevTuples.AddRange(prevAxisTuples);
        //     }
        //     foreach (var tuple in cubeAxis[cubeAxis.Count - 1].Set.Tuples)
        //     {
        //         prevTuples.Add(tuple);
        //     }
        //     resultValues.AddRange(SetAxesValues(cubeAxis.Take(cubeAxis.Count - 1).ToList(), values, prevTuples));
        //     return resultValues;
        // }
        //
        // private List<CubeValue> SetAxisValues(List<CubeQueryTuple> currentAxisTuples, List<CubeQueryValue> values, List<CubeQueryTuple> prevAxisTuples = null)
        // {
        //     List<CubeValue> resultCubeValues= new List<CubeValue>();
        //     List<ICubeMeta> meta = new List<ICubeMeta>();
        //     
        //     if (prevAxisTuples != null)
        //     {
        //         foreach (var prevTuple in prevAxisTuples)
        //         {
        //             foreach (var member in prevTuple.Members)
        //             {
        //                 switch (member.Type)
        //                 {
        //                     case CubeMemberType.Dimension:
        //                     {
        //                         var dimension = GetMetaDimensionValue(member);
        //                         if(dimension == null) continue;
        //                         meta.Add(dimension);
        //                         break;
        //                     }
        //                     case CubeMemberType.Measure:
        //                     {
        //                         var measure = GetMetaMeasure(member);
        //                         if(measure == null) continue;
        //                         meta.Add(measure);
        //                         break;
        //                     }
        //                 }
        //             }
        //         }
        //     }
        //     
        //     foreach(var tuple in currentAxisTuples)
        //     {
        //         foreach (var member in tuple.Members)
        //         {
        //             switch (member.Type)
        //             {
        //                 case CubeMemberType.Dimension:
        //                 {
        //                     var dimension = GetMetaDimensionValue(member);
        //                     if(dimension == null) continue;
        //                     dimensions.Add(dimension);
        //                     break;
        //                 }
        //                 case CubeMemberType.Measure:
        //                 {
        //                     var measure = GetMetaMeasure(member);
        //                     if(measure == null) continue;
        //                     measures.Add(measure);
        //                     break;
        //                 }
        //             }
        //         }
        //
        //         var filteredValues = FilterValues(values, measures, dimensions);
        //         resultCubeValues.Add(AggregateValues(filteredValues));
        //     }
        //     
        //     return resultCubeValues;
        // }
        // private CubeValue AggregateValues(List<CubeQueryValue> values)
        // {
        //     double result = 0;
        //     foreach (var value in values)
        //     {
        //         var valueString = value.Value.ToString();
        //         if(double.TryParse(valueString, out var doubleValue))
        //         {
        //             result += doubleValue;
        //         }
        //     }
        //     return new CubeValue()
        //     {
        //         Value = result,
        //         FmtValue = result.ToString()
        //     };
        // }
        // private List<CubeQueryValue> FilterValues(List<CubeQueryValue> values, List<CubeMetaItem2> measures, List<CubeMetaItem2> dimensions)
        // {
        //     List<CubeQueryValue> filtered = new List<CubeQueryValue>();
        //
        //     foreach (var measure in measures)
        //     {
        //         filtered.AddRange(values.Where(x =>
        //             x.Measure.Key == measure.Key &&
        //             x.Dimensions.Select(x => x.Key).All(x => dimensions.Select(y => y.Key).Contains(x))));
        //     }
        //     return filtered;
        // }
        // private List<CubeMetaItem2> GetCubeDimension(CubeQueryTuple tuple)
        // {
        //     List<CubeMetaItem2> items = new List<CubeMetaItem2>(); 
        //     foreach (var member in tuple.Members)
        //     {
        //         switch (member.Type)
        //         {
        //             case CubeMemberType.Dimension:
        //             {
        //                 var dimensionGroup =
        //                     _cubeMeta.Dimensions.FirstOrDefault(x => x.Dimension.Name == member.Names[0]);
        //                 if(dimensionGroup == null) continue;
        //                 var dimension = dimensionGroup.Values.FirstOrDefault(x => x.Key == member.Names[1]);
        //                 if(dimension == null) continue;
        //                 
        //                 items.Add(dimensionGroup.Dimension);
        //                 items.Add(dimension);
        //                 break;
        //             }
        //             case CubeMemberType.Measure:
        //             {
        //                 var measure = _cubeMeta.Measures.FirstOrDefault(x => x.Measure.Key == member.Names[1]);
        //                 if(measure == null) continue;
        //                 items.Add(measure.Measure);
        //                 break;
        //             }
        //         }
        //     }
        //     return items;
        // }
        //
        // private CubeDimensionMeta? GetMetaDimensionValue(CubeQueryMember member)
        // {
        //     var dimension = _cubeDimensionMeta.FirstOrDefault(x => x.Name == member.Names[0]);
        //     if(dimension == null) return null;
        //     var group = dimension.Clone() as CubeDimensionMeta;
        //     group.Values = new List<CubeDimensionValueMeta>
        //     {
        //         dimension.Values.FirstOrDefault(x => x.Key == member.Names[1])
        //     };
        //     return group;
        // }
        //
        // private CubeMeasureMeta? GetMetaMeasure(CubeQueryMember member)
        // {
        //     return _cubeMeasureMeta.FirstOrDefault(x => x.Key == member.Names[1]);
        // }
        //
        // private List<ICubeMeta> MergeDimension(List<CubeDimensionMeta> dimensions)
        // {
        //     List<ICubeMeta> merged = new List<ICubeMeta>();
        //     foreach (var dimension in dimensions.GroupBy(x => x.Name))
        //     {
        //         merged.AddRange(new CubeDimensionMeta
        //         {
        //             Id = 
        //         });
        //     }
        //     return merged;
        // }
    }
}
