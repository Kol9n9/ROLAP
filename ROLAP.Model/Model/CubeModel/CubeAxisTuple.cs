namespace ROLAP.Model.Model.CubeModel
{
    public class CubeAxisTuple
    {
        public List<CubeAxisMember> Members = new List<CubeAxisMember>();
        public void AddMember(CubeAxisMember member)
        {
            Members.Add(member);
        }
    }
}
