using ROLAP.Model.Interface;
using ROLAP.Model.Models;
using ROLAP.Model.CubeModel;
using ROLAP.TestLocalRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ROLAP.TestModel;

namespace ROLAP.Process
{
    public class CubeProcess
    {
        private CubeQuery cubeQuery = new CubeQuery()
        {
            Axis = new List<AxisQuery> { new AxisQuery()
            {
                AxisItems = new List<AxisItem>()
                {
                    new AxisItem()
                    {
                        Value = new Guid("A0980404-7665-4DB4-8233-39FAEBC4C4E0"),
                        Type = AxisItemType.Measure
                    }
                }
            }, new AxisQuery {
                AxisItems = new List<AxisItem>()
                {
                    new AxisItem()
                    {
                        Value = new Guid("3ac02e75-2988-4bd6-9471-80557bbbcc0d"),
                        Type = AxisItemType.Dimension
                    }
                }
            } }
        };
        private readonly IRepository repository = new Repository();
        public void Process()
        {
            Console.WriteLine("Process start");

            Cube cube = new Cube();
            List<List<Guid>> dimensions = new List<List<Guid>>()
            {
                new List<Guid>{ new Guid("2182f312-6e6b-42f9-adb0-f0165ba617c7") },
                new List<Guid>{ new Guid("3ac02e75-2988-4bd6-9471-80557bbbcc0d"),}
            };
            GenerateAxes(cube, dimensions);
            GenerateValues(cube, dimensions);
        }
        private void GenerateAxes(Cube cube, List<List<Guid>> dimensionsIds)
        {

            foreach (var ids in dimensionsIds)
            {
                GenerateAxis(cube,ids);
            }
        }
        private void GenerateAxis(Cube cube, List<Guid> dimensionIds)
        {
            var dimensions = repository.GetDimensions(dimensionIds);
            CubeAxis cubeAxis = new CubeAxis();
            foreach (var dim in dimensions)
            {
                var member = new CubeAxisMember()
                {
                    Name = dim.Name,
                };
                cubeAxis.AddMember(member);
            }
            cube.AddAxis(cubeAxis);
        }
        private void GenerateValues(Cube cube, List<List<Guid>> dimensionsIds, Guid measureId)
        {
            var values = repository.GetValues(dimensionsIds.SelectMany(x => x).ToList(),measureId);

        }
    }
}
