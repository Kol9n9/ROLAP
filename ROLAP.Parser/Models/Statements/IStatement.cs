using ROLAP.Common.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ROLAP.Common.Model.Interfaces;

namespace ROLAP.Parser.Models.Statements
{
    internal interface IStatement
    {
        CubeQuery Execute(IMappingCubeConfiguration mappingCubeConfiguration);
    }
}
