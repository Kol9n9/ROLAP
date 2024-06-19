using ROLAP.Core.Models.Interfaces;

namespace ROLAP.Core.Models.Interfaces;

public interface ILoader<T> where T : ICubeItem
{
    IContainer Load(IEnumerable<ILoadOptions> options);
}