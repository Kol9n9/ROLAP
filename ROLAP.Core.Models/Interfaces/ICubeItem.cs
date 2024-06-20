using ROLAP.Core.Models.Enums;

namespace ROLAP.Core.Models.Interfaces;

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