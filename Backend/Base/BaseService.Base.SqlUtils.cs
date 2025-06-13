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
    public int GetNr(SqlDataReader r) => Sql.GetInt(r, "Nr");
    public int? GetNrNull(SqlDataReader r) => Sql.GetIntNull(r, "Nr");

    public string GetCode(SqlDataReader r) => GetStringNull(r, "code");
    public string? GetDescription(SqlDataReader r) => GetStringNull(r, "descr");
    public string? GetEncoded(SqlDataReader r) => GetStringNull(r, "encoded");
    public DateTime GetUpdated(SqlDataReader r) => GetDateTime(r, "updated");
    public bool IsActive(SqlDataReader r) => GetBoolean(r, "isActive");
    public string TestActive() => TestActive(null);


	public int GetId(SqlDataReader r, string column) =>Sql.GetId(r, column);
	public int? GetIdNull(SqlDataReader r, string column) => Sql.GetIdNull(r, column);
    public int GetInt(SqlDataReader r, string column) => Sql.GetInt(r, column);
    public int? GetIntNull(SqlDataReader r, string column) => Sql.GetIntNull(r, column);
    public string GetString(SqlDataReader r, string column) => Sql.GetString(r, column);
    public string? GetStringNull(SqlDataReader r, string column) => Sql.GetStringNull(r, column);
    public DateTime GetDateTime(SqlDataReader r, string column) => Sql.GetDateTime(r, column);
    public bool GetBoolean(SqlDataReader r, string column) => Sql.GetBoolean(r, column);
	public string TestActive(string? table) => Sql.TestActive(table);
}

