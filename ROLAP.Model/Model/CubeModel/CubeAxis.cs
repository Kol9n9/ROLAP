namespace ROLAP.Model.Model.CubeModel
{
    public class CubeAxis
    {
        public List<CubeAxisTuple> Tuples { get; set; } = new List<CubeAxisTuple>();
        public void AddTuple(CubeAxisTuple tuple)
        {
            Tuples.Add(tuple);
        }
    }
}
