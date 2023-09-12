using ROLAP.Common.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROLAP.Parser.Models.Statements
{
    internal interface IStatement
    {
        CubeQuery Execute();
    }
}
