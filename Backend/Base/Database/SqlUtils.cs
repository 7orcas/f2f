using GC = Backend.GlobalConstants;

using Microsoft.Data.SqlClient;

/// <summary>
/// Sql utilities
/// Created: June 2025
/// [*Licence*]
/// Author: John Stewart
/// </summary>

namespace Backend.Base.Database
{
    public class SqlUtils
    {
        static public bool ValidateParameter(string parameter) => !string.IsNullOrWhiteSpace(parameter);
        
        static public long GetId(SqlDataReader r) => GetId(r, "id");
        static public long GetId(SqlDataReader r, string column) => (long)r[column];
        static public long? GetIdNull(SqlDataReader r, string column) => r.IsDBNull(r.GetOrdinal(column)) ? null : (long)r[column];
        static public int GetOrgId(SqlDataReader r) => GetInt(r, "orgId");


        static public int GetInt(SqlDataReader r, string column) => (int)r[column];
        static public int? GetIntNull(SqlDataReader r, string column) => r.IsDBNull(r.GetOrdinal(column)) ? null : (int)r[column];
        static public int GetNr(SqlDataReader r) => GetInt(r, "nr");
        static public int? GetNrNull(SqlDataReader r) => GetIntNull(r, "nr");


        static public string GetCode(SqlDataReader r) => GetString(r, "code");
        static public string? GetDescription(SqlDataReader r) => GetStringNull(r, "descr");
        static public string GetString(SqlDataReader r, string column) => (string)r[column];
        static public string? GetStringNull(SqlDataReader r, string column) => r.IsDBNull(r.GetOrdinal(column)) ? null : (string)r[column];

        static public string? GetEncoded(SqlDataReader r) => GetStringNull(r, "encoded");


        static public DateTime GetUpdated(SqlDataReader r) => GetDateTime(r, "updated");
        static public DateTime GetDateTime(SqlDataReader r, string column) => r[column] == DBNull.Value ? DateTime.MinValue : (DateTime)r[column];


        static public bool IsActive(SqlDataReader r) => GetBoolean(r, "isActive");
        static public string TestActive() => TestActive(null);
        static public string TestActive(string? table) => " " + (!string.IsNullOrEmpty(table) ? table + "." : "") + "isActive = 1 ";
        static public bool GetBoolean(SqlDataReader r, string column) => !r.IsDBNull(r.GetOrdinal(column)) && r.GetBoolean(r.GetOrdinal(column));


        static public string Update(string column, int? value) => column + "=" + (value != null? value : "NULL") + ",";
        static public string Update(string column, int value) => column + "=" + value + ",";
        static public string Update(string column, string? value) => column + "=" + (value != null ? "'" + value + "'": "NULL") + ",";
        static public string Update(string column, DateTime? value) => column + "=" + (value != null ? "'" + value.Value.ToString(GC.DateTimeFormat) + "'" : "NULL") + ",";
        static public string Update(string column, bool? value) => column + "=" + (value != null ? (value.Value?1:0)  : "NULL") + ",";
        static public string NoComma(string column) => column.EndsWith(",")? column.Substring(0,column.Length-1) : column;
    }
}
