// #region Copyright
// 
// /****************************************************************************
// *  Copyright (c) 2023 Инком. Все права защищены.
// *
// *  Файл: Helper.cs
// *  Автор: *(slezenko)
// *  Дата создания: 18.09.2023
// *  Назначение: Определение класса Helper.cs
// ****************************************************************************/
// #endregion Copyright

using System.Data;
using Microsoft.EntityFrameworkCore;
using ROLAP.Common.Model.Models;

namespace ROLAP.Repository.Postgre;

public static class Helper
{
    public static List<CubeMetaItem> GetMetaItems(string schema, string table, string idField, string nameField)
    {
        List<CubeMetaItem> results = new List<CubeMetaItem>();
        using (ApplicationDbContext db = new ApplicationDbContext())
        {
            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = $"SELECT * FROM \"{schema}\".\"{table}\"";
                command.CommandType = CommandType.Text;
                db.Database.OpenConnection();
                using (var res = command.ExecuteReader())
                {
                    while (res.Read())
                    {
                        var idOrdinal = res.GetOrdinal(idField);
                        var nameOrdinal = res.GetOrdinal(nameField);
                        results.Add(new CubeMetaItem
                        {
                            Key = res.GetValue(idOrdinal).ToString(),
                            Name = res.GetValue(nameOrdinal).ToString(),
                            Schema = schema,
                            Table = table
                        });
                    }
                }
            }
        }
        return results;
    }
}