namespace ROLAP.Core.Models.Interfaces;

public interface IValueCubeItem : ICubeItem
{
    string GetValue();
    string GetFormattedValue();
}