namespace ROLAP.Common.Model.Models
{
    public class CubeConfiguration
    {
        public string Name { get; set; }
        public List<MeasureConfiguration> Measures { get; set; }
        public List<DimensionConfigurationBase> Dimensions { get; set; }
    }
    public class MeasureConfiguration
    {
        public string Name { get; set; }
        public Guid Key { get; set; } 
        public MeasureValueConfiguration MeasureValue { get; set; }
        public List<string> DimensionNames { get; set; }
    }
    public abstract class DimensionConfigurationBase
    {
        public string Key { get; set; }
        public List<DimensionValueConfiguration> Values { get; set; }
        public TableValuesConfiguration TableValues { get; set; }
    }
    public class OneTableOneDimensions : DimensionConfigurationBase
    { 
        public string Name { get; set; }
    }
    public class OneTableManyDimensions : DimensionConfigurationBase
    {
        public string Schema { get; set; }
        public string Table { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
    public class DimensionValueConfiguration
    {
        public string Name { get; set; }
        public Guid Key { get; set; }
    }
    public class MeasureValueConfiguration
    {
        public string Schema { get; set; }
        public string Table { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
    public class ConnectionWithDimensionConfiguration
    {
        public string Schema { get; set; }
        public string Table { get; set; }
        public string ValueId { get; set; }
        public string DimensionId { get; set; }
    }
    public class TableDimensionConfiguration
    {
        public string Name { get; set; }
        public string Key { get; set; }
        public string Schema { get; set; }
        public string Table { get; set; }
        public TableValuesConfiguration TableValues { get; set; }
    }
    public class TableValuesConfiguration
    {
        public string Name { get; set; }
        public string Key { get; set; }
        public string Schema { get; set; }
        public string Table { get; set; }
        public string ValueConnectionField { get; set; }
        public string DimensionConnectionField { get; set; }
    }
}
