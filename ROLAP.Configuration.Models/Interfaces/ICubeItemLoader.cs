namespace ROLAP.Configuration.Models.Interfaces;

public interface ICubeItemLoader<TRes,TOptions>
{
    IEnumerable<TRes> Load(IEnumerable<TOptions> options);
}