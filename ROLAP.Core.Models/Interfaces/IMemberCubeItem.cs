namespace ROLAP.Core.Models.Interfaces;

public interface IMemberCubeItem : ICubeItem
{
    string GetName();
    string GetKey();
}