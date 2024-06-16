using ROLAP.Core.Models.Interfaces;

namespace ROLAP.Core.Models.Interfaces;

public interface ILoader<T> where T : ICubeItem
{
    IEnumerable<T> Load(IEnumerable<ILoadOptions> options);
}