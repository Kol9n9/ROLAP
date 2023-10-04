using ROLAP.Common.Model.Interfaces;
using ROLAP.Common.Model.Models;

namespace ROLAP.Mapping
{
    public class MappingCubeConfiguration : IMappingCubeConfiguration
    {
        private IRepository _repository;

        public MappingCubeConfiguration(IRepository repository)
        {
            _repository = repository;
        }
        private CubeConfiguration GetCubeConfiguration(string cubeName)
        {
            return new CubeConfiguration
            {
                Name = "Adventure_Cube",
                Measures = new List<MeasureConfiguration>
                {
                    {
                        new MeasureConfiguration
                        {
                            Name = "Количество абитуриентов",
                            MeasureValue = new MeasureValueConfiguration
                            {
                                Id = "NumberOfApplicants",
                                Name = "Количество аббитуриентов",
                                Table = "NumberOfApplicants",
                                Schema = "ROLAP",
                                Value = "Value"
                            },
                            DimensionNames = new List<string>
                            {
                                "University"
                              
                            }
                        }
                    }
                },
                Dimensions = new List<DimensionConfigurationBase>
                {
                    new OneTableOneDimensions
                    {
                        Key = "University",
                        Name = "Университет",
                        TableValues = new TableValuesConfiguration
                        {
                            ValueConnectionField = "UniversityId",
                            Name = "Name",
                            Key = "Id",
                            Schema = "ROLAP",
                            Table = "University"
                        }
                    },
                    new OneTableOneDimensions
                    {
                        Key = "Specialty",
                        Name = "Специальность",
                        TableValues = new TableValuesConfiguration
                        {
                            ValueConnectionField = "SpecialtyId",
                            Name = "Name",
                            Key = "Id",
                            Schema = "ROLAP",
                            Table = "Specialty"
                        }
                    },
                    new OneTableManyDimensions
                    {
                        Key = "Dimension",
                        Name = "Name",
                        Id = "Id",
                        Schema = "ROLAP",
                        Table = "DimensionGroups",
                        TableValues = new TableValuesConfiguration
                        {
                            Name= "Name",
                            Key = "Id",
                            Schema = "ROLAP",
                            Table = "Dimensions",
                            DimensionConnectionField = "MeasureGroupId"
                        }
                    }
                },
            };
        }

        public CubeMeta GetCubeMeta(string cubeName)
        {
            return _repository.GetCubeMeta(GetCubeConfiguration(cubeName));
        }
    }
}