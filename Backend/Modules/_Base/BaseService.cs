
using Microsoft.Data.SqlClient;

namespace Backend.Modules._Base
{
    public class BaseService 
    {
        public T ReadBaseEntity<T> (SqlDataReader r) where T : _BaseEntity, new()
        {
            var entity = new T ();
            entity._OrgId = GetId(r, "_OrgId");
            entity.Id = GetId(r, "Id");
            entity.Descrim = (string)r["Descrim"];
            entity.Code = (string)r["Code"];
            entity.Description = GetString(r, "Descr");
            entity.Encoded = GetString(r, "Encoded");
            //entity.Encoded = (string)r["Encoded"]; // Force Error
            entity.Updated = (DateTime)r["Updated"];

            return entity;
        }

        public string? GetString(SqlDataReader r, string column)
        {
            return r.IsDBNull(r.GetOrdinal(column)) ? (string?)null : (string)r[column];
        }

        public int GetId(SqlDataReader r, string column)
        {
            return (int)r[column];
        }

        public int? GetIdNull(SqlDataReader r, string column)
        {
            return r.IsDBNull(r.GetOrdinal(column)) ? (int?)null : (int)r[column];
        }

        public int GetInt(SqlDataReader r, string column)
        {
            return (int)r[column];
        }

    }
}
