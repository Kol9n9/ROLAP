using ROLAP.Common.Model.Interfaces;
using ROLAP.Common.Model.Models;

namespace ROLAP.TestLocalRepository
{
    public class Repository : IRepository
    {
        private static List<CubeDimension> TestDimensions = new List<CubeDimension>()
        {
            new CubeDimension()
            {
                Id = new Guid("706B4D3A-210A-4C7A-8E6C-36658A9712AB"),
                Name = "Университет"
            },
            new CubeDimension()
            {
                Id = new Guid("62E2E142-8A00-45AB-B8EA-A4CB277EB63F"),
                Name = "Факты и прогнозы"
            },
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
            new CubeMeasure()
            {
                Id = new Guid("A0980404-7665-4DB4-8233-39FAEBC4C4E0"),
                Name = "Количество абитуриентов"
            }
        };
        private static List<CubeValue> TestValues = new List<CubeValue>()
        {
            new CubeValue() // Томск - тгу - план
            {
                Id = Guid.NewGuid(),
                Value = 1000,
                MeasureId = new Guid("A0980404-7665-4DB4-8233-39FAEBC4C4E0"),
                Dimensions = new List<Guid>
                {
                    new Guid("706B4D3A-210A-4C7A-8E6C-36658A9712AB"),
                    new Guid("3ac02e75-2988-4bd6-9471-80557bbbcc0d"),
                    new Guid("2182f312-6e6b-42f9-adb0-f0165ba617c7"),
                    new Guid("62E2E142-8A00-45AB-B8EA-A4CB277EB63F"),
                    new Guid("FC9122BD-4075-42AC-8F29-B7CC44C843D0")
                }
            },
            new CubeValue() // Томск - тгу - факт
            {
                Id = Guid.NewGuid(),
                Value = 950,
                MeasureId = new Guid("A0980404-7665-4DB4-8233-39FAEBC4C4E0"),
                Dimensions = new List<Guid>
                {
                    new Guid("706B4D3A-210A-4C7A-8E6C-36658A9712AB"),
                    new Guid("3ac02e75-2988-4bd6-9471-80557bbbcc0d"),
                    new Guid("2182f312-6e6b-42f9-adb0-f0165ba617c7"),
                    new Guid("62E2E142-8A00-45AB-B8EA-A4CB277EB63F"),
                    new Guid("34476B59-5EF1-4AF7-AFA4-3CD0A17E2CA8")                
                }
            },
            new CubeValue() // Томск - тпу - план
            {
                Id = Guid.NewGuid(),
                Value = 1000,
                MeasureId = new Guid("A0980404-7665-4DB4-8233-39FAEBC4C4E0"),
                Dimensions = new List<Guid>
                {
                    new Guid("706B4D3A-210A-4C7A-8E6C-36658A9712AB"),
                    new Guid("3ac02e75-2988-4bd6-9471-80557bbbcc0d"),
                    new Guid("902c2d8a-b6ac-4732-9918-6637af85dcba"),
                    new Guid("62E2E142-8A00-45AB-B8EA-A4CB277EB63F"),
                    new Guid("FC9122BD-4075-42AC-8F29-B7CC44C843D0")
                }
            },

            new CubeValue() // Томск - тпу - факт
            {
                Id = Guid.NewGuid(),
                Value = 900,
                MeasureId = new Guid("A0980404-7665-4DB4-8233-39FAEBC4C4E0"),
                Dimensions = new List<Guid>
                {
                    new Guid("706B4D3A-210A-4C7A-8E6C-36658A9712AB"),
                    new Guid("3ac02e75-2988-4bd6-9471-80557bbbcc0d"),
                    new Guid("902c2d8a-b6ac-4732-9918-6637af85dcba"),
                    new Guid("62E2E142-8A00-45AB-B8EA-A4CB277EB63F"),
                    new Guid("34476B59-5EF1-4AF7-AFA4-3CD0A17E2CA8")
                }
            },

            new CubeValue() // Новосибирск - нгу - план
            {
                Id = Guid.NewGuid(),
                Value = 1000,
                MeasureId = new Guid("A0980404-7665-4DB4-8233-39FAEBC4C4E0"),
                Dimensions = new List<Guid>
                {
                    new Guid("706B4D3A-210A-4C7A-8E6C-36658A9712AB"),
                    new Guid("82e6587a-6350-4cb5-ba12-b18174aaec26"),
                    new Guid("5f4d8716-b3c8-4e95-b791-23830c271345"),
                    new Guid("62E2E142-8A00-45AB-B8EA-A4CB277EB63F"),
                    new Guid("FC9122BD-4075-42AC-8F29-B7CC44C843D0")
                }
            },
            new CubeValue() // Новосибирск - нгу - факт
            {
                Id = Guid.NewGuid(),
                Value = 950,
                MeasureId = new Guid("A0980404-7665-4DB4-8233-39FAEBC4C4E0"),
                Dimensions = new List<Guid>
                {
                    new Guid("706B4D3A-210A-4C7A-8E6C-36658A9712AB"),
                    new Guid("82e6587a-6350-4cb5-ba12-b18174aaec26"),
                    new Guid("5f4d8716-b3c8-4e95-b791-23830c271345"),
                    new Guid("62E2E142-8A00-45AB-B8EA-A4CB277EB63F"),
                    new Guid("34476B59-5EF1-4AF7-AFA4-3CD0A17E2CA8")
                }
            },
            new CubeValue() // Новосибирск - нпу - план
            {
                Id = Guid.NewGuid(),
                Value = 1000,
                MeasureId = new Guid("A0980404-7665-4DB4-8233-39FAEBC4C4E0"),
                Dimensions = new List<Guid>
                {
                    new Guid("706B4D3A-210A-4C7A-8E6C-36658A9712AB"),
                    new Guid("82e6587a-6350-4cb5-ba12-b18174aaec26"),
                    new Guid("56A531BB-0AB1-49DF-BF46-57BFE16283B0"),
                    new Guid("62E2E142-8A00-45AB-B8EA-A4CB277EB63F"),
                    new Guid("FC9122BD-4075-42AC-8F29-B7CC44C843D0")
                }
            },

            new CubeValue() // Новосибирск - нпу - факт
            {
                Id = Guid.NewGuid(),
                Value = 900,
                MeasureId = new Guid("A0980404-7665-4DB4-8233-39FAEBC4C4E0"),
                Dimensions = new List<Guid>
                {
                    new Guid("706B4D3A-210A-4C7A-8E6C-36658A9712AB"),
                    new Guid("82e6587a-6350-4cb5-ba12-b18174aaec26"),
                    new Guid("56A531BB-0AB1-49DF-BF46-57BFE16283B0"),
                    new Guid("62E2E142-8A00-45AB-B8EA-A4CB277EB63F"),
                    new Guid("34476B59-5EF1-4AF7-AFA4-3CD0A17E2CA8")
                }
            }
        };

        public List<CubeDimension> GetDimensions(List<Guid> dimensionIds)
        {
            return TestDimensions.Where(x => dimensionIds.Contains(x.Id)).ToList();
        }
        public List<CubeMeasure> GetMeasures(List<Guid> measuresId)
        {
            return TestMeasures.Where(x => measuresId.Contains(x.Id)).ToList();
        }
        public List<CubeValue> GetValues (List<List<Guid>> dimensionIds, List<Guid> measureIds)
        {
            return TestValues.Where(w => dimensionIds.Any(x => x.All(y => w.Dimensions.Contains(y)) && (!measureIds.Any() || measureIds.Any(m => m == w.MeasureId)))).ToList();
        }

        public CubeMeta GetCubeMeta(CubeConfiguration configuration)
        {
            throw new NotImplementedException();
        }
    }
}