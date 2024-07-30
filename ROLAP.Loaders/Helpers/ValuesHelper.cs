using ROLAP.Core.Models.Interfaces.Loader;
using ROLAP.Core.Models.Model.Query;
using ROLAP.Core.Models.Interfaces.Container;
using ROLAP.Core.Models.Interfaces.CubeItem;
using ROLAP.Loaders.Models.Containers;
using ROLAP.Loaders.Models.CubeItems;

namespace ROLAP.Loaders.Helpers;

public static class ValuesHelper
{
    public static IEnumerable<IValueCubeItem> LoadValues(IEnumerable<CubeItemTuple> tupleItems, IEnumerable<CubeItemTuple> whereTupleItems)
    {
        List<IValueCubeItem> values = new List<IValueCubeItem>();

        List<IContainer> measureContainers = new List<IContainer>();
        List<IContainer> dimensionsContainers = new List<IContainer>();

        foreach (var tupleItem in tupleItems)
        {
            measureContainers.AddRange(tupleItem.Members.OfType<MeasureContainer>());
            dimensionsContainers.AddRange(tupleItem.Members.OfType<DimensionContainer>());
        }
        
        foreach (var tupleItem in whereTupleItems)
        {
            measureContainers.AddRange(tupleItem.Members.OfType<MeasureContainer>());
            dimensionsContainers.AddRange(tupleItem.Members.OfType<DimensionContainer>());
        }

        List<ILoadOptions> optionsList = LoadOptionsHelper.GetValueOptions(dimensionsContainers).ToList();

        foreach (var measure in measureContainers.SelectMany(x => x.GetValues<IMeasureCubeItem>()))
        {
            values.AddRange(measure.GetLoader().Load(optionsList));
        }
       
        return values;
    }
}