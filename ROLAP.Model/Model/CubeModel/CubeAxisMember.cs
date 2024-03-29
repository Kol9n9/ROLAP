﻿using ROLAP.Common.Model.Enums;

namespace ROLAP.Model.Model.CubeModel
{
    public class CubeAxisMember
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public CubeMemberType Type { get; set; }
    }
}
