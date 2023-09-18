// #region Copyright
// 
// /****************************************************************************
// *  Copyright (c) 2023 Инком. Все права защищены.
// *
// *  Файл: IMappingCubeConfiguration.cs
// *  Автор: *(slezenko)
// *  Дата создания: 18.09.2023
// *  Назначение: Определение класса IMappingCubeConfiguration.cs
// ****************************************************************************/
// #endregion Copyright

using ROLAP.Common.Model.Models;

namespace ROLAP.Common.Model.Interfaces;

public interface IMappingCubeConfiguration
{
    CubeMeta GetCubeMeta(string cubeName);
}