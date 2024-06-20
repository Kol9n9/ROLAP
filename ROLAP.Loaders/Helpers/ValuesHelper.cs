using ROLAP.Core.Models.Interfaces;
using ROLAP.Core.Models.Model.Query;
using ROLAP.Models.Models.IContainers;
using ROLAP.Models.Models.ICubeItems;

namespace ROLAP.Loaders.Helpers;

public static class ValuesHelper
{
    public static IEnumerable<ICubeItem> LoadValues(IEnumerable<CubeItemTuple> tupleItems, IEnumerable<CubeItemTuple> whereTupleItems)
    {
        List<ICubeItem> values = new List<ICubeItem>();

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

        foreach (var measure in measureContainers.SelectMany(x => x.GetValues<MeasureCubeItem>()))
        {
            values.AddRange(measure.GetLoader().Load(optionsList).GetValues<ValueCubeItem>());
        }
       
        return values;
    }

    public static ICubeItem AggregatedValues(IEnumerable<ICubeItem> values)
    {
        var first = values.FirstOrDefault();
        return first ?? new ValueCubeItem("", "", null, null);
    }
}