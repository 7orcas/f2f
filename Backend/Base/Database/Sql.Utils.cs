

using Microsoft.Data.SqlClient;

/// <summary>
/// Sql utilities
/// Created: June 2025
/// [*Licence*]
/// Author: John Stewart
/// </summary>

namespace Backend.Base.Database
{
    public partial class Sql
    {
        static public bool ValidateParameter(string parameter) => !string.IsNullOrWhiteSpace(parameter);
        
        static public int GetId(SqlDataReader r, string column) => (int)r[column];
        static public int? GetIdNull(SqlDataReader r, string column) => r.IsDBNull(r.GetOrdinal(column)) ? null : (int)r[column];
        static public int GetInt(SqlDataReader r, string column) => (int)r[column];
        static public int? GetIntNull(SqlDataReader r, string column) => r.IsDBNull(r.GetOrdinal(column)) ? null : (int)r[column];
        

        static public string GetString(SqlDataReader r, string column) => (string)r[column];
        static public string? GetStringNull(SqlDataReader r, string column) => r.IsDBNull(r.GetOrdinal(column)) ? null : (string)r[column];

        static public bool GetBoolean(SqlDataReader r, string column) => !r.IsDBNull(r.GetOrdinal(column)) && r.GetBoolean(r.GetOrdinal(column));
        static public string TestActive(string? table) => " " + (!string.IsNullOrEmpty(table) ? table + "." : "") + "isActive = 1 ";
        
        static public DateTime GetDateTime(SqlDataReader r, string column) => r[column] == DBNull.Value ? DateTime.MinValue : (DateTime)r[column];

    }
}
