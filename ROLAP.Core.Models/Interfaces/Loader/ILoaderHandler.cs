using ROLAP.Core.Models.Interfaces.CubeItem;

namespace ROLAP.Core.Models.Interfaces.Loader;

public interface ILoaderHandler<T, TOptions>
    where TOptions: ILoadOptions
    where T : ICubeItem
{
    IEnumerable<T> Load(TOptions options);
}