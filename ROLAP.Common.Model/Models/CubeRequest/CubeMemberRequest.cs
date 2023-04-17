using ROLAP.Common.Model.Enums;

namespace ROLAP.Common.Model.Models.CubeRequest
{
    public class CubeMemberRequest
    {
        public Guid Id { get; set; }
        public CubeMemberType Type { get; set; }
    }
}
