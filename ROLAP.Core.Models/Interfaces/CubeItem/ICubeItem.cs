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
    ICubeItem Clone(bool withValues);
}