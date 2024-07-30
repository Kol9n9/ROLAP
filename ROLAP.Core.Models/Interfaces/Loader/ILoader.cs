using ROLAP.Core.Models.Interfaces.Container;
using ROLAP.Core.Models.Interfaces.CubeItem;

namespace ROLAP.Core.Models.Interfaces.Loader;

public interface ILoader<T> where T : ICubeItem
{
    IEnumerable<T> Load(IEnumerable<ILoadOptions> options);
}