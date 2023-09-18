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
                            Dimensions = new List<DimensionConfiguration>
                            {
                                new DimensionConfiguration
                                {
                                    Name = "Университет",
                                    TableValues = new TableValuesConfiguration
                                    {
                                        ConnectionField = "UniversityId",
                                        Name = "Name",
                                        Key = "Id",
                                        Schema = "ROLAP",
                                        Table = "University"
                                    }
                                },
                                new DimensionConfiguration
                                {
                                    Name = "Специальность",
                                    TableValues = new TableValuesConfiguration
                                    {
                                        ConnectionField = "SpecialtyId",
                                        Name = "Name",
                                        Key = "Id",
                                        Schema = "ROLAP",
                                        Table = "Specialty"
                                    }
                                }
                            },
                            MeasureValue = new MeasureValueConfiguration
                            {
                                Id = "",
                                Name = "Количество аббитуриентов",
                                Table = "NumberOfApplicants",
                                Schema = "ROLAP",
                                Value = "Value"
                            }
                        }
                    }
                }
            };
        }

        public CubeMeta GetCubeMeta(string cubeName)
        {
            return _repository.GetCubeMeta(GetCubeConfiguration(cubeName));
        }
    }
}