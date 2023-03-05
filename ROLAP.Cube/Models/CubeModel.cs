using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ROLAP.Models.Axes;
using ROLAP.Models.Cells;
using ROLAP.Models.Info;

namespace ROLAP.Models
{
    /// <summary>
    /// Основная модель OLAP - куба
    /// </summary>
    public class CubeModel
    {
        private static string CubeNamespace = "urn:schemas-microsoft-com:xml-analysis:mddataset";
        public OlapInfoModel OlapInfo { get; set; }
        public AxesModel Axes { get; set; }
        public CellDataModel CellData { get; set; }
    }
}
