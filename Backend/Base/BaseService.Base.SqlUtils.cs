using Microsoft.Data.SqlClient;

/// <summary>
/// Basic Sql functions
/// Created:16/6/24
/// [*Licence*]
/// Author: John Stewart
/// </summary>

namespace Backend.Base;

public abstract partial class BaseService
{
    public int GetId(SqlDataReader r) => GetId(r, "id");
    public int GetOrgId(SqlDataReader r) => GetId(r, "orgId");
    public string GetCode(SqlDataReader r) => GetString(r, "code");
    public string? GetDescription(SqlDataReader r) => GetString(r, "descr");
    public string? GetEncoded(SqlDataReader r) => GetString(r, "encoded");
    public DateTime GetUpdated(SqlDataReader r) => GetDateTime(r, "updated");
    public bool IsActive(SqlDataReader r) => GetBoolean(r, "isActive");
    public string TestActive() => TestActive(null);
	public string TestActive(string table) => Sql.TestActive(table);
    public string? GetString(SqlDataReader r, string column) => Sql.GetString(r, column);
	public int GetId(SqlDataReader r, string column) =>Sql.GetId(r, column);
	public int? GetIdNull(SqlDataReader r, string column) => Sql.GetIdNull(r, column);
	public int GetInt(SqlDataReader r, string column) => Sql.GetInt(r, column);
	public bool GetBoolean(SqlDataReader r, string column) => Sql.GetBoolean(r, column);
	public DateTime GetDateTime(SqlDataReader r, string column) => Sql.GetDateTime(r, column);
}

