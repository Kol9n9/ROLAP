using ROLAP.Common.Model.Interfaces;
using ROLAP.Common.Model.Models.Meta;
using ROLAP.CubeConfiguration.Models.Dimension;
using ROLAP.CubeConfiguration.Models.Measure;

namespace ROLAP.CubeConfiguration;

public static class MapperCubeConfiguration
{
    private static Models.CubeConfiguration MapCubeConfiguration()
    {
        return new Models.CubeConfiguration
        {
            Name = "Adventure_Cube",
            MeasureConfigurations = new List<IMeasureConfiguration>
            {
                new StaticMeasureConfiguration
                {
                    Name = "Количество абитуриентов",
                    ValueConnection = new MeasureValueConnection
                    {
                        SchemaName = "ROLAP",
                        TableName = "NumberOfApplicants",
                        ValueFieldName = "Value"
                    }
                },
                new DynamicMeasureConfiguration
                {
                    SchemaName = "ROLAP",
                    TableName = "Measures",
                    IdField = "Id",
                    NameField = "Name",
                    ValueConnection = new MeasureValueConnection
                    {
                        SchemaName = "ROLAP",
                        TableName = "Values",
                        ValueConnectionName = "MeasureId",
                        ValueFieldName = "Value"
                    }
                }
            },
            DimensionGroupConfigurations = new List<IDimensionConfiguration>
            {
                new StaticDimensionConfiguration
                {
                    Name = "Университет",
                    Values = new List<IDimensionValueConfiguration>
                    {
                        new DynamicDimensionValueConfiguration
                        {
                            SchemaName = "ROLAP",
                            TableName = "University",
                            IdField = "Id",
                            NameField = "Name",
                            ValueConnectionField = "UniversityId",
                        }
                    }
                },
                new StaticDimensionConfiguration
                {
                    Name = "Специальность",
                    Values = new List<IDimensionValueConfiguration>
                    {
                        new DynamicDimensionValueConfiguration
                        {
                            SchemaName = "ROLAP",
                            TableName = "Specialty",
                            IdField = "Id",
                            NameField = "Name",
                            ValueConnectionField = "SpecialtyId"
                        }
                    }
                },
                new DynamicDimensionConfiguration
                {
                    SchemaName = "ROLAP",
                    TableName = "DimensionGroups",
                    IdField = "Id",
                    NameField = "Name",
                    Values = new List<IDimensionValueConfiguration>
                    {
                        new DynamicDimensionValueConfiguration
                        {
                            SchemaName = "ROLAP",
                            TableName = "Dimensions",
                            IdField = "Id",
                            NameField = "Name",
                            ValueConnectionField = "DimensionId",
                            DimensionValueConnectionField = "MeasureGroupId"
                        }
                    }
                }
            }
        };
    }
    public static List<ICubeMeta> GetCubeConfiguration(IRepository repository)
    {
        return LoadCubeConfiguration(repository,MapCubeConfiguration());
    }

    private static List<ICubeMeta> LoadCubeConfiguration(IRepository repository, Models.CubeConfiguration cubeConfiguration)
    {
        List<ICubeMeta> meta = new List<ICubeMeta>();
        meta.AddRange(LoadMeasures(repository,cubeConfiguration.MeasureConfigurations));
        meta.AddRange(LoadDimensions(repository,cubeConfiguration.DimensionGroupConfigurations));
        return meta;
    }

    private static List<ICubeMeta> LoadMeasures(IRepository repository, List<IMeasureConfiguration> measureConfigurations)
    {
        List<ICubeMeta> results = new List<ICubeMeta>();
        foreach (var measureConfiguration in measureConfigurations)
        {
            if (measureConfiguration is StaticMeasureConfiguration staticMeasureConfiguration)
            {
                results.Add(new CubeMeasureMeta
                {
                    Key = staticMeasureConfiguration.Key,
                    Name = staticMeasureConfiguration.Name,
                    ValueConnection = new CubeMeasureValueConnectionMeta
                    {
                        SchemaName = staticMeasureConfiguration.ValueConnection.SchemaName,
                        TableName = staticMeasureConfiguration.ValueConnection.TableName,
                        ValueConnectionField = staticMeasureConfiguration.ValueConnection.ValueConnectionName,
                        ValueField = staticMeasureConfiguration.ValueConnection.ValueFieldName
                    }
                });
            } 
            else if (measureConfiguration is DynamicMeasureConfiguration dynamicMeasureConfiguration)
            {
                foreach (var cubeMetaData in repository.LoadMetaData(dynamicMeasureConfiguration.SchemaName,dynamicMeasureConfiguration.TableName,dynamicMeasureConfiguration.IdField,dynamicMeasureConfiguration.KeyField,dynamicMeasureConfiguration.NameField))
                {
                    results.Add(new CubeMeasureMeta
                    {
                        Id = cubeMetaData.Id,
                        Key = cubeMetaData.Key,
                        Name = cubeMetaData.Name,
                        ValueConnection = new CubeMeasureValueConnectionMeta
                        {
                            SchemaName = dynamicMeasureConfiguration.ValueConnection.SchemaName,
                            TableName = dynamicMeasureConfiguration.ValueConnection.TableName,
                            ValueConnectionField = dynamicMeasureConfiguration.ValueConnection.ValueConnectionName,
                            ValueField = dynamicMeasureConfiguration.ValueConnection.ValueFieldName
                        }
                    });
                }
            }
        }

        return results;
    }

    private static List<ICubeMeta> LoadDimensions(IRepository repository, List<IDimensionConfiguration> dimensionGroupConfigurations)
    {
        List<ICubeMeta> res = new List<ICubeMeta>();
        foreach (var dimensionGroupConfiguration in dimensionGroupConfigurations)
        {
            if (dimensionGroupConfiguration is StaticDimensionConfiguration staticDimensionGroupConfiguration)
            {
                res.Add(new CubeDimensionMeta
                {
                    Key = staticDimensionGroupConfiguration.Key,
                    Name = staticDimensionGroupConfiguration.Name,
                    Values = LoadDimensionValues(repository,staticDimensionGroupConfiguration.Values,null)
                });
            } 
            else if (dimensionGroupConfiguration is DynamicDimensionConfiguration dynamicDimensionGroupConfiguration)
            {
                foreach (var cubeMetaData in repository.LoadMetaData(dynamicDimensionGroupConfiguration.SchemaName,dynamicDimensionGroupConfiguration.TableName,dynamicDimensionGroupConfiguration.IdField, dynamicDimensionGroupConfiguration.KeyField,dynamicDimensionGroupConfiguration.NameField))
                {
                    res.Add(new CubeDimensionMeta
                    {
                        Id = cubeMetaData.Id,
                        Key = cubeMetaData.Key,
                        Name = cubeMetaData.Name,
                        Values = LoadDimensionValues(repository,dynamicDimensionGroupConfiguration.Values, cubeMetaData.Id)
                    });
                }
            }
        }

        return res;
    }

    private static List<CubeDimensionValueMeta> LoadDimensionValues(IRepository repository, List<IDimensionValueConfiguration> dimensionConfigurations, string? connectionFieldValue)
    {
        List<CubeDimensionValueMeta> res = new List<CubeDimensionValueMeta>();
        foreach (var dimensionConfiguration in dimensionConfigurations)
        {
            if (dimensionConfiguration is StaticDimensionValueConfiguration staticDimensionConfiguration)
            {
                res.Add(new CubeDimensionValueMeta
                {
                    Key = staticDimensionConfiguration.Key,
                    Id = staticDimensionConfiguration.Id,
                    Name = staticDimensionConfiguration.Name,
                    ValueConnectionField = staticDimensionConfiguration.ValueConnectionField
                });
            }
            else if (dimensionConfiguration is DynamicDimensionValueConfiguration dynamicDimensionConfiguration)
            {
                foreach (var cubeMetaData in repository.LoadMetaData(dynamicDimensionConfiguration.SchemaName,dynamicDimensionConfiguration.TableName,dynamicDimensionConfiguration.IdField,dynamicDimensionConfiguration.KeyField,dynamicDimensionConfiguration.NameField, dynamicDimensionConfiguration.DimensionValueConnectionField, connectionFieldValue))
                {
                    res.Add(new CubeDimensionValueMeta
                    {
                        Id = cubeMetaData.Id,
                        Key = cubeMetaData.Key,
                        Name = cubeMetaData.Name,
                        ValueConnectionField = dynamicDimensionConfiguration.ValueConnectionField
                    });
                }
            }
        }

        return res;
    }
    
}