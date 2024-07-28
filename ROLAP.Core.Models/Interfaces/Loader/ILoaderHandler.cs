namespace ROLAP.Core.Models.Interfaces.Loader;

public interface ILoaderHandler<T, TOptions>
    where TOptions: ILoadOptions
{
    IEnumerable<T> Load(TOptions options);
}