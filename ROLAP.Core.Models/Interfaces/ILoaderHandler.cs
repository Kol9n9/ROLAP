using ROLAP.Core.Models.Interfaces;

namespace ROLAP.Core.Models.Interfaces;

public interface ILoaderHandler<TITem, TOptions>
    where TITem: ICubeItem
    where TOptions: ILoadOptions
{
    IEnumerable<TITem> Load(TOptions options);
}