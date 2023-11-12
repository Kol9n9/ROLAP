using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ROLAP.Common.Model.Interfaces;
using ROLAP.Common.Model.Models;
using ROLAP.Common.Model.Models.Meta;

namespace ROLAP.Repository.Postgre;

public class PostgreRepository : IRepository
{
    public List<object> GetValues(string schemaName, string tableName, string valueField, List<Tuple<string,string>>? dimensions, string? connectionField = null, string? connectionFieldValue = null)
    {
        List<object> values = new List<object>();
        using (ApplicationDbContext db = new ApplicationDbContext())
        {
            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                bool isConnectionField = false;
                var query = $"SELECT \"{valueField}\" FROM \"{schemaName}\".\"{tableName}\"";
                if (!string.IsNullOrWhiteSpace(connectionField))
                {
                    if (string.IsNullOrWhiteSpace(connectionFieldValue))
                        throw new ArgumentNullException(nameof(connectionFieldValue));

                    query += $" WHERE \"{connectionField}\"='{connectionFieldValue}'";
                    isConnectionField = true;
                }

                if (dimensions != null && dimensions.Any())
                {
                    if (!isConnectionField)
                    {
                        query += " WHERE";
                    }
                    else
                    {
                        query += " AND";
                    }

                    query += $"\"{dimensions[0].Item1}\"='{dimensions[0].Item2}'";
                    foreach (var dimension in dimensions.Skip(1))
                    {
                        query += $" AND \"{dimension.Item1}\"='{dimension.Item2}'";
                    }
                }
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                db.Database.OpenConnection();
                using (var res = command.ExecuteReader())
                {
                    while (res.Read())
                    {
                        values.Add(res.GetValue(0));
                    }
                }
            }
        }
        return values;
    }

    public List<CubeMetaData> LoadMetaData(string schemaName, string tableName, string idField, string keyField, string nameField, string? connectionField = null, string? connectionFieldValue = null)
    {
        List<CubeMetaData> results = new List<CubeMetaData>();
        using (ApplicationDbContext db = new ApplicationDbContext())
        {
            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                var query = $"SELECT * FROM \"{schemaName}\".\"{tableName}\"";
                if (!string.IsNullOrWhiteSpace(connectionField))
                {
                    if (string.IsNullOrWhiteSpace(connectionFieldValue))
                        throw new ArgumentNullException(nameof(connectionFieldValue));

                    query += $" WHERE \"{connectionField}\"='{connectionFieldValue}'";
                }
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                db.Database.OpenConnection();
                using (var res = command.ExecuteReader())
                {
                    while (res.Read())
                    {
                        var idOrdinal = res.GetOrdinal(idField);
                        var nameOrdinal = res.GetOrdinal(nameField);
                        results.Add(new CubeMetaData
                        {
                            Id = res.GetValue(idOrdinal)?.ToString(),
                            Name = res.GetValue(nameOrdinal)?.ToString()
                        });
                    }
                }
            }
        }

        return results;
    }
}