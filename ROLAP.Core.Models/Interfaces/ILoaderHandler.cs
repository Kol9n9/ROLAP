using ROLAP.Core.Models.Interfaces;

namespace ROLAP.Core.Models.Interfaces;

public interface ILoaderHandler<T, TOptions>
    where TOptions: ILoadOptions
{
    IEnumerable<T> Load(TOptions options);
}