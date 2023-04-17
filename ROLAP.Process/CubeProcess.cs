using ROLAP.Model.Interface;
using ROLAP.Model.Models;
using ROLAP.Model.CubeModel;
using ROLAP.TestLocalRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ROLAP.Model.CubeRequest;

namespace ROLAP.Process
{
    public class CubeProcess
    {
        //private readonly CubeRequest request = new CubeRequest()
        //{
        //    Axes = new List<CubeAxisRequest> { 
        //        new CubeAxisRequest(){ // Столбцы
        //            //Tuples = new List<CubeAxisTupleRequest> {
        //            //    new CubeAxisTupleRequest()
        //            //    {
        //            //        Members = new List<CubeMemberRequest>
        //            //        {
        //            //            new CubeMemberRequest()
        //            //            {
        //            //                Id = new Guid("706B4D3A-210A-4C7A-8E6C-36658A9712AB"), // Университет
        //            //                Type = CubeMemberType.Dimension
        //            //            },
        //            //            new CubeMemberRequest()
        //            //            {
        //            //                Id = new Guid("2182f312-6e6b-42f9-adb0-f0165ba617c7"), // ТГУ
        //            //                Type = CubeMemberType.Dimension
        //            //            },
        //            //            new CubeMemberRequest()
        //            //            {
        //            //                Id = new Guid("902c2d8a-b6ac-4732-9918-6637af85dcba"), // ТПУ
        //            //                Type = CubeMemberType.Dimension
        //            //            }
        //            //        }
        //            //    },
        //            //}
        //            Tuples = new List<CubeAxisTupleRequest> {
        //                new CubeAxisTupleRequest()
        //                {
        //                    Members = new List<CubeMemberRequest>
        //                    {
        //                        new CubeMemberRequest()
        //                        {
        //                            Id = new Guid("706B4D3A-210A-4C7A-8E6C-36658A9712AB"), // Университет
        //                            Type = CubeMemberType.Dimension
        //                        },
        //                        new CubeMemberRequest()
        //                        {
        //                            Id = new Guid("62E2E142-8A00-45AB-B8EA-A4CB277EB63F"), // Факты и прогнозы
        //                            Type= CubeMemberType.Dimension
        //                        }
        //                    }
        //                },
        //                new CubeAxisTupleRequest()
        //                {
        //                    Members = new List<CubeMemberRequest>
        //                    {
        //                        new CubeMemberRequest()
        //                        {
        //                            Id = new Guid("2182f312-6e6b-42f9-adb0-f0165ba617c7"), // ТГУ
        //                            Type = CubeMemberType.Dimension
        //                        },
        //                        new CubeMemberRequest()
        //                        {
        //                            Id = new Guid("62E2E142-8A00-45AB-B8EA-A4CB277EB63F"), // Факт и прогнозы
        //                            Type = CubeMemberType.Dimension
        //                        }                              
        //                    }
        //                },
        //                new CubeAxisTupleRequest()
        //                {
        //                    Members = new List<CubeMemberRequest>
        //                    {
        //                        new CubeMemberRequest()
        //                        {
        //                            Id = new Guid("2182f312-6e6b-42f9-adb0-f0165ba617c7"), // ТГУ
        //                            Type = CubeMemberType.Dimension
        //                        },
        //                        new CubeMemberRequest()
        //                        {
        //                            Id = new Guid("34476B59-5EF1-4AF7-AFA4-3CD0A17E2CA8"), // Факт
        //                            Type = CubeMemberType.Dimension
        //                        }
        //                    }
        //                },
        //                new CubeAxisTupleRequest()
        //                {
        //                    Members = new List<CubeMemberRequest>
        //                    {
        //                        new CubeMemberRequest()
        //                        {
        //                            Id = new Guid("2182f312-6e6b-42f9-adb0-f0165ba617c7"), // ТГУ
        //                            Type = CubeMemberType.Dimension
        //                        },
        //                        new CubeMemberRequest()
        //                        {
        //                            Id = new Guid("FC9122BD-4075-42AC-8F29-B7CC44C843D0"), // План
        //                            Type = CubeMemberType.Dimension
        //                        }
        //                    }
        //                },
        //                new CubeAxisTupleRequest()
        //                {
        //                    Members = new List<CubeMemberRequest>
        //                    {
        //                        new CubeMemberRequest()
        //                        {
        //                            Id = new Guid("902c2d8a-b6ac-4732-9918-6637af85dcba"), // ТПУ
        //                            Type = CubeMemberType.Dimension
        //                        },
        //                        new CubeMemberRequest()
        //                        {
        //                            Id = new Guid("62E2E142-8A00-45AB-B8EA-A4CB277EB63F"), // Факт и прогнозы
        //                            Type = CubeMemberType.Dimension
        //                        }
        //                    }
        //                },
        //                new CubeAxisTupleRequest()
        //                {
        //                    Members = new List<CubeMemberRequest>
        //                    {
        //                        new CubeMemberRequest()
        //                        {
        //                            Id = new Guid("902c2d8a-b6ac-4732-9918-6637af85dcba"), // ТПУ
        //                            Type = CubeMemberType.Dimension
        //                        },
        //                        new CubeMemberRequest()
        //                        {
        //                            Id = new Guid("34476B59-5EF1-4AF7-AFA4-3CD0A17E2CA8"), // Факт
        //                            Type = CubeMemberType.Dimension
        //                        }
        //                    }
        //                },
        //                new CubeAxisTupleRequest()
        //                {
        //                    Members = new List<CubeMemberRequest>
        //                    {
        //                        new CubeMemberRequest()
        //                        {
        //                            Id = new Guid("902c2d8a-b6ac-4732-9918-6637af85dcba"), // ТПУ
        //                            Type = CubeMemberType.Dimension
        //                        },
        //                        new CubeMemberRequest()
        //                        {
        //                            Id = new Guid("FC9122BD-4075-42AC-8F29-B7CC44C843D0"), // План
        //                            Type = CubeMemberType.Dimension
        //                        }
        //                    }
        //                }
        //            }
        //        },
        //        new CubeAxisRequest(){ // Строки
        //            Tuples = new List<CubeAxisTupleRequest> {
        //                new CubeAxisTupleRequest()
        //                {
        //                    Members = new List<CubeMemberRequest>
        //                    {
        //                        new CubeMemberRequest()
        //                        {
        //                            Id = new Guid("3ac02e75-2988-4bd6-9471-80557bbbcc0d"), // Томск
        //                            Type = CubeMemberType.Dimension
        //                        }
        //                    }
        //                }
        //            }
        //        },
        //    }
        //};
        private readonly IRepository repository = new Repository();
        public void Process(CubeRequest request)
        {
            Console.WriteLine("Process start");
            Cube cube = new Cube();
            cube.Axes = GenerateAxes(request);
            var values = GetValues(cube.Axes);
            var cubeValues = SetAxesValues(values,cube.Axes);
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
                        Type = (CubeMemberType2)member.Type
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
                        if(member.Type == CubeMemberType2.Dimension)
                        {
                             dimensionIds.Add(member.Id);
                        } 
                        else if(member.Type == CubeMemberType2.Measure)
                        {
                            measureIds.Add(member.Id);
                        }
                    }
                    dimensionTuplesIds.Add(dimensionIds);
                }
            }
            return repository.GetValues(dimensionTuplesIds, measureIds);
        }
        private List<CubeValue> SetAxesValues(List<CubeValue> values, List<CubeAxis> axes, List<CubeAxisTuple> prevAxisTuples = null)
        {
            List<CubeValue> resultValues = new List<CubeValue>();
            if (axes.Count == 1)
            {
                return SetAxisValues(values, axes[0].Tuples, prevAxisTuples);
            }

            foreach(var tuple in axes[axes.Count - 1].Tuples)
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

        private List<CubeValue> SetAxisValues(List<CubeValue> values, List<CubeAxisTuple> currentAxisTuples, List<CubeAxisTuple> prevAxisTuples = null)
        {
            List<CubeValue> resultCubeValues= new List<CubeValue>();

            foreach(var tuple in currentAxisTuples)
            {
                List<Guid> dimensionIds = new List<Guid>();
                List<Guid> measureIds = new List<Guid>();
                foreach (var member in tuple.Members)
                {
                    if(member.Type == CubeMemberType2.Dimension)
                    {
                        dimensionIds.Add(member.Id);
                    } 
                    else if(member.Type == CubeMemberType2.Measure)
                    {
                        measureIds.Add(member.Id);
                    }
                }
                if(prevAxisTuples != null)
                {
                    foreach(var prevTuple in prevAxisTuples)
                    {
                        foreach (var member in prevTuple.Members)
                        {
                            if (member.Type == CubeMemberType2.Dimension)
                            {
                                dimensionIds.Add(member.Id);
                            }
                            else if (member.Type == CubeMemberType2.Measure)
                            {
                                measureIds.Add(member.Id);
                            }
                        }
                    }
                }
                var filteredValues = values.Where(v => dimensionIds.All(d => v.Dimensions.Contains(d)) && (!measureIds.Any() || measureIds.All(m => v.MeasureId == m))).ToList();
                resultCubeValues.Add(AggregateValues(filteredValues));
            }
            return resultCubeValues;
        }
        private CubeValue AggregateValues(List<CubeValue> values)
        {
            double result = 0;
            foreach (var value in values)
            {
                result += value.Value;
            }
            return new CubeValue()
            {
                Value = result
            };
        }
    }
}
