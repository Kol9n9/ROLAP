using ROLAP.Configuration.Loaders.Base;
using ROLAP.Configuration.Models.Models;
using ROLAP.Parser;
using ROLAP.Processor;
using ROLAP.Processor.Interfaces;


string mdx = "SELECT CrossJoin([Dimension].&[62E2E142-8A00-45AB-B8EA-A4CB277EB63F],{[Dimension].&[34476B59-5EF1-4AF7-AFA4-3CD0A17E2CA8],[Dimension].&[FC9122BD-4075-42AC-8F29-B7CC44C843D0]}) ON 0, " +
             "{[Dimension].&[3ac02e75-2988-4bd6-9471-80557bbbcc0d],[Dimension].&[82e6587a-6350-4cb5-ba12-b18174aaec26]} ON 1 " +
             "FROM [Adventure_Cube]";
        
string mdx2 =
    "SELECT {[Университет].[ТГУ],[Университет].[ТПУ],[Measure].&[NumberOfApplicants]} ON 0, [Специальность].[Прикладная информатика] ON 1 FROM [Adventure_Cube]";
        
string mdx3 =
    "SELECT CrossJoin([Университет].[ТГУ],[Measure].&[NumberOfApplicants]) ON 0 FROM [Adventure_Cube]";
        
string mdx4 = "SELECT CrossJoin([Measure].[Доход],{[Факты и прогнозы].[План],[Факты и прогнозы].[Факт]}) ON 0, {[ОКВЭД].[Тест],[ОКВЭД].[Проверка]} ON 1 FROM [Adventure_Cube]";

string mdx5 = "SELECT {[Университет].[ТГУ]} ON 1 FROM [Adventure_Cube]";

string mdx6 = "SELECT [Measure].[Прибыль] ON 0 FROM [Ade]";

string mdx7 = "SELECT CrossJoin([ОКВЭД].[01 Растениеводство и животноводство],{[Measure].[Прибыль],[Measure].[Расход]}) ON 0 FROM [asd]";
string mdx8 =
    "SELECT CROSSJOIN({[Страна].[Россия],[Страна].[Казахстан]},{[Measure].[Прибыль],[Measure].[Расход]}) ON 0, {[ОКВЭД].[01 Растениеводство и животноводство],[ОКВЭД].[06 Добыча сырой нефти и природного газа]} ON 1 FROM [asd]";

IProcessor processor = new Processor(new CubeConfigurationStore(new CubeConfigurationLoader()));
//processor.ProcessQuery(mdx);
// processor.ProcessQuery(mdx2);
// processor.ProcessQuery(mdx3);
// processor.ProcessQuery(mdx4);
// processor.ProcessQuery(mdx5);
 
processor.ProcessQuery(mdx8);