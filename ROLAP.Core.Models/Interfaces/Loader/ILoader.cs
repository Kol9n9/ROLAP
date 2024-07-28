using ROLAP.Core.Models.Interfaces.Container;

namespace ROLAP.Core.Models.Interfaces.Loader;

public interface ILoader<T>
{
    IContainer Load(IEnumerable<ILoadOptions> options);
}