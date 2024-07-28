namespace ROLAP.Core.Models.Interfaces.CubeItem;

public interface IMemberCubeItem : ICubeItem
{
    string GetName();
    string GetKey();
}