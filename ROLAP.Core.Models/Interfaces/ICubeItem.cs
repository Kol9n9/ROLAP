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
    /// <param name="withInnerValues">Клонировать внутрение значения (по возможности)</param>
    /// <returns></returns>
    ICubeItem Clone(bool withInnerValues = true);

    /// <summary>
    /// Клонировать элемент и привести к типу T
    /// </summary>
    /// <param name="withInnerValues"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    T Clone<T>(bool withInnerValues = true) where T : ICubeItem;
}