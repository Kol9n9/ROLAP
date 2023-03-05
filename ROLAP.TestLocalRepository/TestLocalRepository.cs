using ROLAP.Model.Interface;
using ROLAP.Model.Models;
using ROLAP.Utils;
using System.Linq;

namespace ROLAP.TestLocalRepository
{
    public class TestLocalRepository : IRepository
    {
        private static List<CubeDimension> TestDimensions = new List<CubeDimension>()
        {
            new CubeDimension()
            {
                Id = new Guid("34476B59-5EF1-4AF7-AFA4-3CD0A17E2CA8"),
                Name = "Факт"
            },
            new CubeDimension()
            {
                Id = new Guid("FC9122BD-4075-42AC-8F29-B7CC44C843D0"),
                Name = "План"
            },
            new CubeDimension()
            {
                Id = new Guid("3ac02e75-2988-4bd6-9471-80557bbbcc0d"),
                Name = "Томск"
            },
            new CubeDimension()
            {
                Id = new Guid("2182f312-6e6b-42f9-adb0-f0165ba617c7"),
                Name = "ТГУ"
            },
            new CubeDimension()
            {
                Id = new Guid("902c2d8a-b6ac-4732-9918-6637af85dcba"),
                Name = "ТПУ"
            },
           
            new CubeDimension()
            {
                Id = new Guid("82e6587a-6350-4cb5-ba12-b18174aaec26"),
                Name = "Новосибирск"
            },    
            new CubeDimension()
            {
                Id = new Guid("5f4d8716-b3c8-4e95-b791-23830c271345"),
                Name = "НГУ"
            },
            new CubeDimension()
            {
                Id = new Guid("56A531BB-0AB1-49DF-BF46-57BFE16283B0"),
                Name = "НПУ"
            },
        };
        private static List<CubeMeasure> TestMeasures = new List<CubeMeasure>()
        {
            new CubeMeasure() // Томск - тгу - план
            {
                Id = new Guid(),
                Value = 1000,
                Dimensions = new List<Guid>
                {
                    new Guid("3ac02e75-2988-4bd6-9471-80557bbbcc0d"),
                    new Guid("2182f312-6e6b-42f9-adb0-f0165ba617c7"),
                    new Guid("FC9122BD-4075-42AC-8F29-B7CC44C843D0")
                }
            },
            new CubeMeasure() // Томск - тгу - факт
            {
                Id = new Guid(),
                Value = 950,
                Dimensions = new List<Guid>
                {
                    new Guid("3ac02e75-2988-4bd6-9471-80557bbbcc0d"),
                    new Guid("2182f312-6e6b-42f9-adb0-f0165ba617c7"),
                    new Guid("34476B59-5EF1-4AF7-AFA4-3CD0A17E2CA8")                
                }
            },
            new CubeMeasure() // Томск - тпу - план
            {
                Id = new Guid(),
                Value = 1000,
                Dimensions = new List<Guid>
                {
                    new Guid("3ac02e75-2988-4bd6-9471-80557bbbcc0d"),
                    new Guid("902c2d8a-b6ac-4732-9918-6637af85dcba"),
                    new Guid("FC9122BD-4075-42AC-8F29-B7CC44C843D0")
                }
            },

            new CubeMeasure() // Томск - тпу - факт
            {
                Id = new Guid(),
                Value = 900,
                Dimensions = new List<Guid>
                {
                    new Guid("3ac02e75-2988-4bd6-9471-80557bbbcc0d"),
                    new Guid("902c2d8a-b6ac-4732-9918-6637af85dcba"),
                    new Guid("34476B59-5EF1-4AF7-AFA4-3CD0A17E2CA8")
                }
            },

            new CubeMeasure() // Новосибирск - нгу - план
            {
                Id = new Guid(),
                Value = 1000,
                Dimensions = new List<Guid>
                {
                    new Guid("82e6587a-6350-4cb5-ba12-b18174aaec26"),
                    new Guid("5f4d8716-b3c8-4e95-b791-23830c271345"),
                    new Guid("FC9122BD-4075-42AC-8F29-B7CC44C843D0")
                }
            },
            new CubeMeasure() // Новосибирск - нгу - факт
            {
                Id = new Guid(),
                Value = 950,
                Dimensions = new List<Guid>
                {
                    new Guid("82e6587a-6350-4cb5-ba12-b18174aaec26"),
                    new Guid("5f4d8716-b3c8-4e95-b791-23830c271345"),
                    new Guid("34476B59-5EF1-4AF7-AFA4-3CD0A17E2CA8")
                }
            },
            new CubeMeasure() // Новосибирск - нпу - план
            {
                Id = new Guid(),
                Value = 1000,
                Dimensions = new List<Guid>
                {
                    new Guid("82e6587a-6350-4cb5-ba12-b18174aaec26"),
                    new Guid("56A531BB-0AB1-49DF-BF46-57BFE16283B0"),
                    new Guid("FC9122BD-4075-42AC-8F29-B7CC44C843D0")
                }
            },

            new CubeMeasure() // Новосибирск - нпу - факт
            {
                Id = new Guid(),
                Value = 900,
                Dimensions = new List<Guid>
                {
                    new Guid("82e6587a-6350-4cb5-ba12-b18174aaec26"),
                    new Guid("56A531BB-0AB1-49DF-BF46-57BFE16283B0"),
                    new Guid("34476B59-5EF1-4AF7-AFA4-3CD0A17E2CA8")
                }
            }
        };

        public List<CubeDimension> GetDimensions(List<Guid> dimensionIds)
        {
            return TestDimensions.Where(x => dimensionIds.Contains(x.Id)).ToList();
        }

        public List<CubeMeasure> GetMeasures(List<Guid> dimensionIds)
        {
            return TestMeasures.Where(w => dimensionIds.All(x => w.Dimensions.Contains(x))).ToList();
        }
    }
}