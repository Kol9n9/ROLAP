// #region Copyright
// 
// /****************************************************************************
// *  Copyright (c) 2023 Инком. Все права защищены.
// *
// *  Файл: ApplicationDbContext.cs
// *  Автор: *(slezenko)
// *  Дата создания: 18.09.2023
// *  Назначение: Определение класса ApplicationDbContext.cs
// ****************************************************************************/
// #endregion Copyright

using Microsoft.EntityFrameworkCore;

namespace ROLAP.Repository.Postgre;

public class ApplicationDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=ROLAP;Username=postgres;Password=123456");
    }
}