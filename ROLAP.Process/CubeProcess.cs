using ROLAP.Common.Model.Enums;
using ROLAP.Common.Model.Interfaces;
using ROLAP.Common.Model.Models;
using ROLAP.Common.Model.Models.CubeRequest;
using ROLAP.Model.Model;
using ROLAP.Model.Model.CubeModel;
using ROLAP.TestLocalRepository;

namespace ROLAP.Process
{
    public class CubeProcess
    {
        private readonly IRepository repository = new Repository();
        public void Process(string mdx)
        {
            //Console.WriteLine("Process start");
            //var request = Parser.Parser.Parse(mdx);
            //Cube cube = new Cube();
            //cube.Axes = GenerateAxes(request);
            //var values = GetValues(cube.Axes);
            //cube.Values = SetAxesValues(values, cube.Axes);
        }
        private List<CubeAxis> GenerateAxes(CubeRequest request)
        {
            List<CubeAxis> axes = new List<CubeAxis>();
            foreach (var requestAxis in request.Axes)
            {
                axes.Add(GenerateAxis(requestAxis));
            }
            return axes;
        }
        private CubeAxis GenerateAxis(CubeAxisRequest axis)
        {
            CubeAxis resultAxis = new CubeAxis();
            List<Guid> dimensionIds = axis.Tuples.Select(x => x.Members.Where(m => m.Type == CubeMemberType.Dimension).Select(m => m.Id).ToList()).SelectMany(x => x).Distinct().ToList();
            var cubeDimensions = repository.GetDimensions(dimensionIds);        
            foreach (var tuple in axis.Tuples)
            {
                CubeAxisTuple cubeTuple = new CubeAxisTuple();
                foreach (var member in tuple.Members)
                {
                    var cubeDimension = cubeDimensions.FirstOrDefault(x => x.Id == member.Id);
                    if(cubeDimension == null)
                    {
                        throw new Exception("Измерение не найдено");
                    }
                    cubeTuple.AddMember(new CubeAxisMember() { 
                        Id = member.Id,
                        Name = cubeDimension.Name,
                        Type = member.Type
                    });
                }
                resultAxis.AddTuple(cubeTuple);
            }
            return resultAxis;
        }
        private List<CubeValue> GetValues(List<CubeAxis> cubeAxes)
        {
            List<List<Guid>> dimensionTuplesIds = new List<List<Guid>>();
            List<Guid> measureIds = new List<Guid>();
            foreach (var cubeAxis in cubeAxes)
            {
                foreach(var tuple in cubeAxis.Tuples)
                {
                    List<Guid> dimensionIds = new List<Guid>();
                    foreach (var member in tuple.Members)
                    {
                        if(member.Type == CubeMemberType.Dimension)
                        {
                             dimensionIds.Add(member.Id);
                        } 
                        else if(member.Type == CubeMemberType.Measure)
                        {
                            measureIds.Add(member.Id);
                        }
                    }
                    dimensionTuplesIds.Add(dimensionIds);
                }
            }
            return repository.GetValues(dimensionTuplesIds, measureIds);
        }
        private List<AggregateValue> SetAxesValues(List<CubeValue> values, List<CubeAxis> axes, List<CubeAxisTuple> prevAxisTuples = null)
        {
            if (axes.Count == 1)
            {
                return SetAxisValues(values, axes[0].Tuples, prevAxisTuples);
            }
            List<AggregateValue> resultValues = new List<AggregateValue>();
            foreach (var tuple in axes[axes.Count - 1].Tuples)
            {
                List<CubeAxisTuple> prevTuples = new List<CubeAxisTuple>();
                if(prevAxisTuples != null)
                {
                    prevTuples.AddRange(prevAxisTuples);
                }
                prevTuples.Add(tuple);
                resultValues.AddRange(SetAxesValues(values,axes.Take(axes.Count - 1).ToList(), prevTuples));
            }
            return resultValues;
        }

        private List<AggregateValue> SetAxisValues(List<CubeValue> values, List<CubeAxisTuple> currentAxisTuples, List<CubeAxisTuple> prevAxisTuples = null)
        {
            List<AggregateValue> resultCubeValues= new List<AggregateValue>();

            List<Guid> prevTuplesDimensions = new List<Guid>();
            List<Guid> prevTuplesMeasures = new List<Guid>();

            if(prevAxisTuples!= null)
            {
                foreach (var prevTuple in prevAxisTuples)
                {
                    foreach (var member in prevTuple.Members)
                    {
                        if (member.Type == CubeMemberType.Dimension)
                        {
                            prevTuplesDimensions.Add(member.Id);
                        }
                        else if (member.Type == CubeMemberType.Measure)
                        {
                            prevTuplesMeasures.Add(member.Id);
                        }
                    }
                }
            }

            foreach(var tuple in currentAxisTuples)
            {
                List<Guid> dimensionIds = new List<Guid>();
                List<Guid> measureIds = new List<Guid>();
                foreach (var member in tuple.Members)
                {
                    if(member.Type == CubeMemberType.Dimension)
                    {
                        dimensionIds.Add(member.Id);
                    } 
                    else if(member.Type == CubeMemberType.Measure)
                    {
                        measureIds.Add(member.Id);
                    }
                }
                dimensionIds.AddRange(prevTuplesDimensions);
                measureIds.AddRange(prevTuplesMeasures);
                var filteredValues = values.Where(v => dimensionIds.All(d => v.Dimensions.Contains(d)) && (!measureIds.Any() || measureIds.All(m => v.MeasureId == m))).ToList();
                resultCubeValues.Add(AggregateValues(filteredValues));
            }
            return resultCubeValues;
        }
        private AggregateValue AggregateValues(List<CubeValue> values)
        {
            decimal result = 0;
            foreach (var value in values)
            {
                result += value.Value;
            }
            return new AggregateValue()
            {
                Value = result,
                FormattedValue = result.ToString()
            };
        }
    }
}
