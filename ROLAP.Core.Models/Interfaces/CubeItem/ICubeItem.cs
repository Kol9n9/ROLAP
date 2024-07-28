namespace ROLAP.Core.Models.Interfaces.CubeItem;

/// <summary>
/// Абстракция для элемента
/// </summary>
public interface ICubeItem
{
    /// <summary>
    /// Клонировать элемент
    /// </summary>
    /// <returns></returns>
    T Clone<T>(bool withValues) where T : ICubeItem;
}