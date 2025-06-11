using Microsoft.Data.SqlClient;

namespace Backend.Base;

public abstract partial class BaseService
{
    public int GetId(SqlDataReader r)
    {
        return GetId(r, "id");
    }

    public int GetOrgId(SqlDataReader r)
    {
        return GetId(r, "orgId");
    }

    public string GetCode(SqlDataReader r)
    {
        return GetString(r, "code");
    }

    public string? GetDescription(SqlDataReader r)
    {
        return GetString(r, "descr");
    }

    public string? GetEncoded(SqlDataReader r)
    {
        return GetString(r, "encoded");
    }

    public DateTime GetUpdated(SqlDataReader r)
    {
        return GetDateTime(r, "updated");
    }

    public bool IsActive(SqlDataReader r)
    {
        return GetBoolean(r, "isActive");
    }
    
    public string TestActive()
	{
		return TestActive(null);
	}
	public string TestActive(string table)
    {
        return Sql.TestActive(table);
    }

    public string? GetString(SqlDataReader r, string column)
	{
		return Sql.GetString(r, column);
	}

	public int GetId(SqlDataReader r, string column)
	{
		return Sql.GetId(r, column);
	}

	public int? GetIdNull(SqlDataReader r, string column)
	{
		return Sql.GetIdNull(r, column);
	}

	public int GetInt(SqlDataReader r, string column)
	{
		return Sql.GetInt(r, column);
	}

	public bool GetBoolean(SqlDataReader r, string column)
	{
		return Sql.GetBoolean(r, column);
	}

	public DateTime GetDateTime(SqlDataReader r, string column)
	{
		return Sql.GetDateTime(r, column);
	}

}

