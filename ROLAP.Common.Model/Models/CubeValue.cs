namespace ROLAP.Common.Model.Models
{
    public class CubeValue
    {
        public Guid Id { get; set; }
        public decimal Value { get; set; }
        public List<Guid> Dimensions { get; set; }
        public Guid MeasureId { get; set; }
    }
}
