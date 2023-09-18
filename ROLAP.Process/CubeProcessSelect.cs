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
            //var values = GetValues(cube.Axes);
            //cube.Values = SetAxesValues(values, cube.Axes);
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
            List<List<string>> dimensionsIds = MergeDimensions(axis.Set.Tuples.Select(x => x.Members.Where(y => y.Type == CubeMemberType.Dimension)).SelectMany(x => x).Select(x => x.Names).ToList());

            var cubeDimensions = GetCubeDimensions(dimensionsIds);        
            //foreach (var tuple in axis.Tuples)
            //{
            //    CubeAxisTuple cubeTuple = new CubeAxisTuple();
            //    foreach (var member in tuple.Members)
            //    {
            //        var cubeDimension = cubeDimensions.FirstOrDefault(x => x.Id == member.Id);
            //        if(cubeDimension == null)
            //        {
            //            throw new Exception("Измерение не найдено");
            //        }
            //        cubeTuple.AddMember(new CubeAxisMember() { 
            //            Id = member.Id,
            //            Name = cubeDimension.Name,
            //            Type = member.Type
            //        });
            //    }
            //    resultAxis.AddTuple(cubeTuple);
            //}
            return resultAxis;
        }
        //private List<CubeValue> GetValues(List<CubeAxis> cubeAxes)
        //{
        //    List<List<Guid>> dimensionTuplesIds = new List<List<Guid>>();
        //    List<Guid> measureIds = new List<Guid>();
        //    foreach (var cubeAxis in cubeAxes)
        //    {
        //        foreach(var tuple in cubeAxis.Tuples)
        //        {
        //            List<Guid> dimensionIds = new List<Guid>();
        //            foreach (var member in tuple.Members)
        //            {
        //                if(member.Type == CubeMemberType.Dimension)
        //                {
        //                     dimensionIds.Add(member.Id);
        //                } 
        //                else if(member.Type == CubeMemberType.Measure)
        //                {
        //                    measureIds.Add(member.Id);
        //                }
        //            }
        //            dimensionTuplesIds.Add(dimensionIds);
        //        }
        //    }
        //    return repository.GetValues(dimensionTuplesIds, measureIds);
        //}
        //private List<AggregateValue> SetAxesValues(List<CubeValue> values, List<CubeAxis> axes, List<CubeAxisTuple> prevAxisTuples = null)
        //{
        //    if (axes.Count == 1)
        //    {
        //        return SetAxisValues(values, axes[0].Tuples, prevAxisTuples);
        //    }
        //    List<AggregateValue> resultValues = new List<AggregateValue>();
        //    foreach (var tuple in axes[axes.Count - 1].Tuples)
        //    {
        //        List<CubeAxisTuple> prevTuples = new List<CubeAxisTuple>();
        //        if(prevAxisTuples != null)
        //        {
        //            prevTuples.AddRange(prevAxisTuples);
        //        }
        //        prevTuples.Add(tuple);
        //        resultValues.AddRange(SetAxesValues(values,axes.Take(axes.Count - 1).ToList(), prevTuples));
        //    }
        //    return resultValues;
        //}

        //private List<AggregateValue> SetAxisValues(List<CubeValue> values, List<CubeAxisTuple> currentAxisTuples, List<CubeAxisTuple> prevAxisTuples = null)
        //{
        //    List<AggregateValue> resultCubeValues= new List<AggregateValue>();

        //    List<Guid> prevTuplesDimensions = new List<Guid>();
        //    List<Guid> prevTuplesMeasures = new List<Guid>();

        //    if(prevAxisTuples!= null)
        //    {
        //        foreach (var prevTuple in prevAxisTuples)
        //        {
        //            foreach (var member in prevTuple.Members)
        //            {
        //                if (member.Type == CubeMemberType.Dimension)
        //                {
        //                    prevTuplesDimensions.Add(member.Id);
        //                }
        //                else if (member.Type == CubeMemberType.Measure)
        //                {
        //                    prevTuplesMeasures.Add(member.Id);
        //                }
        //            }
        //        }
        //    }

        //    foreach(var tuple in currentAxisTuples)
        //    {
        //        List<Guid> dimensionIds = new List<Guid>();
        //        List<Guid> measureIds = new List<Guid>();
        //        foreach (var member in tuple.Members)
        //        {
        //            if(member.Type == CubeMemberType.Dimension)
        //            {
        //                dimensionIds.Add(member.Id);
        //            } 
        //            else if(member.Type == CubeMemberType.Measure)
        //            {
        //                measureIds.Add(member.Id);
        //            }
        //        }
        //        dimensionIds.AddRange(prevTuplesDimensions);
        //        measureIds.AddRange(prevTuplesMeasures);
        //        var filteredValues = values.Where(v => dimensionIds.All(d => v.Dimensions.Contains(d)) && (!measureIds.Any() || measureIds.All(m => v.MeasureId == m))).ToList();
        //        resultCubeValues.Add(AggregateValues(filteredValues));
        //    }
        //    return resultCubeValues;
        //}
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


        private List<CubeMetaDimension> GetCubeDimensions(List<List<string>> dimensionIds)
        {
            List<CubeMetaDimension> cubeDimensions = new List <CubeMetaDimension>();

           
            foreach (var dimensionId in dimensionIds)
            {
                //List<CubeMetaDimension> cubeDimension = new List<CubeMetaDimension>();
                //cubeDimension.AddRange(_cubeMeta.Dimensions.Where(x => x.Dimension.Name == dimensionId[0]).ToList());
                var cubeDimension = new List<CubeMetaDimension>(_cubeMeta.Dimensions.Where(x => x.Dimension.Name == dimensionId[0]).Select(x => x).ToList());
                foreach (var item in cubeDimension)
                {
                    var values = item.Values.Where(x => x.Key == dimensionId[1]).ToList();
                    item.Values = new List<CubeMetaItem>(values);
                }
                cubeDimensions.AddRange(cubeDimension);
            }
            return cubeDimensions;
        }
        private List<List<string>> MergeDimensions(List<List<string>> dimensions)
        {
            return dimensions;
            var result = new List<List<string>>();
            for(int i = 0; i < dimensions.Count; i++)
            {
                var dimension = dimensions[i];
                var otherDimensions = dimensions.Where(x => x[0] == dimension[0]).ToList();
                dimension.AddRange(otherDimensions.Select(x => x[1]));
                dimensions.RemoveAll(x => x[0] == dimension[0]);
            }

            return result;
        }
    }
}
