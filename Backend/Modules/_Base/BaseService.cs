
using Microsoft.Data.SqlClient;

namespace Backend.Modules._Base
{
    public class BaseService 
    {
        public T ReadBaseEntity<T> (SqlDataReader r) where T : _BaseEntity, new()
        {
            var entity = new T ();
            entity.OrgId = GetId(r, "OrgId");
            entity.Id = GetId(r, "Id");
            entity.Code = (string)r["Code"];
            entity.Description = GetString(r, "Descr");
            entity.Encoded = GetString(r, "Encoded");
            //entity.Encoded = (string)r["Encoded"]; // Force Error, testing
            entity.Updated = GetDateTime(r, "Updated");
            entity.IsActive = GetBoolean(r, "Encoded");

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

        public bool GetBoolean(SqlDataReader r, string column)
        {
            return !r.IsDBNull(r.GetOrdinal(column)) && r.GetBoolean(r.GetOrdinal(column));
        }

        public DateTime GetDateTime(SqlDataReader r, string column)
        {
            return r[column] == DBNull.Value ? DateTime.MinValue : (DateTime)r[column];
        }

    }
}
