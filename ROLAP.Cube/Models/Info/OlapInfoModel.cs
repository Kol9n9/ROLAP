using ROLAP.Cube.Models.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROLAP.Models.Info
{
    /// <summary>
    /// Контейнер содержащий мета информацию про OLAP куб. Обязательный
    /// </summary>
    public class OlapInfoModel
    {
        public CubeInfoModel CubeInfo { get; set; }
    }
}
