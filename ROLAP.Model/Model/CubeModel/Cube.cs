namespace ROLAP.Model.Model.CubeModel
{
    public class Cube
    {
        public List<CubeAxis> Axes { get; set; } = new List<CubeAxis>();
        public List<AggregateValue> Values { get; set; } = new List<AggregateValue> { };
        public void AddAxis(CubeAxis axis)
        {
            Axes.Add(axis);
        }

    }
}
