using ROLAP.Core.Models.Interfaces.Loader;
using ROLAP.Core.Models.Model.Query;
using ROLAP.Core.Models.Interfaces.CubeItem;

namespace ROLAP.Loaders.Helpers;

public static class ValuesHelper
{
    public static IEnumerable<IValueCubeItem> LoadValues(IEnumerable<CubeItemSet> sets, CubeItemSet? whereSet)
    {
        List<IValueCubeItem> values = new List<IValueCubeItem>();

        List<IMeasureCubeItem> measureCubeItems = new List<IMeasureCubeItem>();
        List<IDimensionCubeItem> dimensionCubeItems = new List<IDimensionCubeItem>();
        
        foreach (var set in sets)
        {
            measureCubeItems.AddRange(SelectFromSet<IMeasureCubeItem>(set));
            dimensionCubeItems.AddRange(SelectFromSet<IDimensionCubeItem>(set));
        }

        if (whereSet is not null)
        {
            measureCubeItems.AddRange(SelectFromSet<IMeasureCubeItem>(whereSet));
            dimensionCubeItems.AddRange(SelectFromSet<IDimensionCubeItem>(whereSet));
        }
        
        List<ILoadOptions> optionsList = LoadOptionsHelper.GetValueOptions(dimensionCubeItems).ToList();
        
        foreach (var measure in measureCubeItems)
        {
            values.AddRange(measure.GetLoader().Load(optionsList));
        }

        return values;
    }

    private static IEnumerable<T> SelectFromSet<T>(CubeItemSet set)
    {
        return set.Tuples.Where(x => x.Members.First() is T).SelectMany(x => x.Members.Cast<T>());
    }
}