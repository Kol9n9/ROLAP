using ROLAP.Common.Model.Interfaces;
using ROLAP.Common.Model.Models;
using ROLAP.Process.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ROLAP.Process
{
    internal class CubeProcessSelect
    {
        private readonly IRepository _repository;
        private readonly CubeQuery _cubeQuery;
        private readonly CubeMeta _cubeMeta;
        public CubeProcessSelect(IRepository repository, CubeQuery cubeQuery, CubeMeta cubeMeta) {
            _repository = repository;
            _cubeQuery = cubeQuery;
            _cubeMeta = cubeMeta;
        }
        public Cube Process()
        {
            Cube cube = new Cube();
            cube.Axes = GenerateAxes(_cubeQuery);
            var values = GetValues(_cubeQuery);
            SetAxesValues(_cubeQuery.Axes, values);
            return null;
        }
        private List<CubeAxis> GenerateAxes(CubeQuery request)
        {
            List<CubeAxis> axes = new List<CubeAxis>();
            foreach (var requestAxis in request.Axes)
            {
                axes.Add(GenerateAxis(requestAxis));
            }
            return axes;
        }
        private CubeAxis GenerateAxis(CubeQueryAxis axis)
        {
            CubeAxis resultAxis = new CubeAxis();
           
            foreach (var tuple in axis.Set.Tuples)
            {
                var cubeMetaItems = GetCubeDimension(tuple);
                CubeTuple newTuple = new CubeTuple();
                foreach (var cubeMetaItem in cubeMetaItems)
                {
                    newTuple.Members.Add(new CubeMember
                    {
                        Name = cubeMetaItem.Name
                    });
                }
                resultAxis.Tuples.Add(newTuple);
            }
            
            return resultAxis;
        }
        
        private List<object> GetValues(CubeQuery cubeAxes)
        {
            List<CubeMetaItem> dimensions = new List<CubeMetaItem>();
            List<CubeMetaItem> measures = new List<CubeMetaItem>();
            foreach (var axis in cubeAxes.Axes) 
            {
                foreach (var tuple in axis.Set.Tuples)
                {
                    foreach (var member in tuple.Members)
                    {
                        switch (member.Type)
                        {
                            case CubeMemberType.Dimension:
                            {
                                var dimensionGroup =
                                    _cubeMeta.Dimensions.FirstOrDefault(x => x.Dimension.Name == member.Names[0]);
                                if(dimensionGroup == null) continue;
                                var dimension = dimensionGroup.Values.FirstOrDefault(x => x.Key == member.Names[1]);
                                if(dimension == null) continue;
                                dimensions.Add(dimension);
                                break;
                            }
                            case CubeMemberType.Measure:
                            {
                                var measure =
                                    _cubeMeta.Measures.FirstOrDefault(x => x.Measure.Key == member.Names[1]);
                                if(measure == null) continue;
                                measures.Add(measure.Measure);
                                break;
                            }
                        }
                    }
                }
            }
            return _repository.GetValues(measures,dimensions);
        }
        private List<object> SetAxesValues(List<CubeQueryAxis> cubeAxis, List<object> values, List<CubeQueryTuple> prevAxisTuples = null)
        {
            if (cubeAxis.Count == 1)
            {
                return SetAxisValues(cubeAxis[0].Set.Tuples, values, prevAxisTuples);
            }
            List<object> resultValues = new List<object>();
            foreach (var tuple in cubeAxis[cubeAxis.Count - 1].Set.Tuples)
            {
                List<CubeQueryTuple> prevTuples = new List<CubeQueryTuple>();
                if(prevAxisTuples != null)
                {
                    prevTuples.AddRange(prevAxisTuples);
                }
                prevTuples.Add(tuple);
                resultValues.AddRange(SetAxesValues(cubeAxis.Take(cubeAxis.Count - 1).ToList(), values, prevTuples));
            }
            return resultValues;
        }

        private List<object> SetAxisValues(List<CubeQueryTuple> currentAxisTuples, List<object> values, List<CubeQueryTuple> prevAxisTuples = null)
        {
            List<object> resultCubeValues= new List<object>();
            //
            // List<Guid> prevTuplesDimensions = new List<Guid>();
            // List<Guid> prevTuplesMeasures = new List<Guid>();
            //
            // if(prevAxisTuples!= null)
            // {
            //     foreach (var prevTuple in prevAxisTuples)
            //     {
            //         foreach (var member in prevTuple.Members)
            //         {
            //             if (member.Type == CubeMemberType.Dimension)
            //             {
            //                 prevTuplesDimensions.Add(member.Id);
            //             }
            //             else if (member.Type == CubeMemberType.Measure)
            //             {
            //                 prevTuplesMeasures.Add(member.Id);
            //             }
            //         }
            //     }
            // }
            //
            // foreach(var tuple in currentAxisTuples)
            // {
            //     List<Guid> dimensionIds = new List<Guid>();
            //     List<Guid> measureIds = new List<Guid>();
            //     foreach (var member in tuple.Members)
            //     {
            //         if(member.Type == CubeMemberType.Dimension)
            //         {
            //             dimensionIds.Add(member.Id);
            //         } 
            //         else if(member.Type == CubeMemberType.Measure)
            //         {
            //             measureIds.Add(member.Id);
            //         }
            //     }
            //     dimensionIds.AddRange(prevTuplesDimensions);
            //     measureIds.AddRange(prevTuplesMeasures);
            //     var filteredValues = values.Where(v => dimensionIds.All(d => v.Dimensions.Contains(d)) && (!measureIds.Any() || measureIds.All(m => v.MeasureId == m))).ToList();
            //     resultCubeValues.Add(AggregateValues(filteredValues));
            // }
            return resultCubeValues;
        }
        //private AggregateValue AggregateValues(List<CubeValue> values)
        //{
        //    decimal result = 0;
        //    foreach (var value in values)
        //    {
        //        result += value.Value;
        //    }
        //    return new AggregateValue()
        //    {
        //        Value = result,
        //        FormattedValue = result.ToString()
        //    };
        //}
        
        private List<CubeMetaItem> GetCubeDimension(CubeQueryTuple tuple)
        {
            List<CubeMetaItem> items = new List<CubeMetaItem>(); 
            foreach (var member in tuple.Members)
            {
                switch (member.Type)
                {
                    case CubeMemberType.Dimension:
                    {
                        var dimensionGroup =
                            _cubeMeta.Dimensions.FirstOrDefault(x => x.Dimension.Name == member.Names[0]);
                        if(dimensionGroup == null) continue;
                        var dimension = dimensionGroup.Values.FirstOrDefault(x => x.Key == member.Names[1]);
                        if(dimension == null) continue;
                        
                        items.Add(dimensionGroup.Dimension);
                        items.Add(dimension);
                        break;
                    }
                    case CubeMemberType.Measure:
                    {
                        var measure = _cubeMeta.Measures.FirstOrDefault(x => x.Measure.Key == member.Names[1]);
                        if(measure == null) continue;
                        items.Add(measure.Measure);
                        break;
                    }
                }
            }

            return items;
        }
    }
}
