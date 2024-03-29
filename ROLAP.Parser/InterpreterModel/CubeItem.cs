﻿using ROLAP.Common.Model.Models.CubeRequest;

namespace ROLAP.Parser.InterpreterModel
{
    internal class CubeItem
    {
        public string CubeName { get; set; }
        public List<AxisItem> Axes { get; set; } = new List<AxisItem>();

        public void Run()
        {
            foreach (AxisItem axis in Axes) 
            {
                axis.Run();
            }
        }
        public CubeRequest GetCubeRequest()
        {
            CubeRequest cubeRequest = new CubeRequest();
            foreach (AxisItem axis in Axes)
            {
                cubeRequest.Axes.Add(new CubeAxisRequest
                {
                    Tuples = axis.GetAxisTuplesRequest()
                });
            }
            return cubeRequest;
        }
    }
}
