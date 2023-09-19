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
    public static List<CubeMetaItem> GetMetaItems(string schema, string table, string idField, string nameField, string connectionField)
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
                            Table = table,
                            ConnectionField = connectionField
                        });
                    }
                }
            }
        }
        return results;
    }

    public static List<object> GetValues(string measureSchema, string measureTable, string measureValueField, List<Tuple<string, string>> dimensionFields)
    {
        List<object> results = new List<object>();
        using (ApplicationDbContext db = new ApplicationDbContext())
        {
            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                string sql = $"SELECT \"{measureValueField}\" FROM \"{measureSchema}\".\"{measureTable}\"";
                
                if (dimensionFields.Any())
                {
                    sql += " WHERE";
                    sql += $" \"{dimensionFields[0].Item1}\"='{dimensionFields[0].Item2}'";
                    foreach (var dimensionField in dimensionFields.Skip(1))
                    {
                        sql += $" AND \"{dimensionField.Item1}\"='{dimensionField.Item2}'";
                    }
                }

                command.CommandText = sql;
                command.CommandType = CommandType.Text;
                db.Database.OpenConnection();
                using (var res = command.ExecuteReader())
                {
                    while (res.Read())
                    {
                        results.Add(res.GetValue(0));
                    }
                }
            }
        }
        return results;
    }
}