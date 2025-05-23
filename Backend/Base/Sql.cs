using Microsoft.Data.SqlClient;

namespace Backend.Base
{
    public class Sql
    {
        static public async Task<bool> Run(string sqlString, Action<SqlDataReader> action, params SqlParameter[] parameters)
        {
            return await Task.Run(() =>
            {
                
//Thread.Sleep(400);
                var connectionString = AppSettings.DBMainConnection;

                SqlConnection connection = null;
                SqlDataReader reader = null;
                try
                {
                    connection = new SqlConnection(connectionString);
                    connection.Open();
                    var command = new SqlCommand(sqlString, connection);

                    if (parameters != null && parameters.Length > 0)
                        command.Parameters.AddRange(parameters);

                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        action(reader);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                    var p = "";
                    for (int i = 0; parameters != null && i < parameters.Length; i++)
                        p += (p.Length > 0 ? "," : "") + parameters[i].TypeName + ":" + parameters[i].Value.ToString();

                    if (p.Length > 0) p = " p -> (" + p + ")";

                    Log.Logger.Error(sqlString +
                        p +
                        " -> " + ex.Message);
                    throw;
                }
                finally
                {
                    if (reader != null) reader.Close();
                    if (connection != null) connection.Close();
                }

                return true;
            });
        }

        static public async Task<bool> Execute(string sqlString)
        {
            return await Task.Run(() =>
            {

                //Thread.Sleep(400);
                var connectionString = AppSettings.DBMainConnection;

                SqlConnection connection = null;
                try
                {
                    connection = new SqlConnection(connectionString);
                    connection.Open();
                    var command = new SqlCommand(sqlString, connection);
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message); 
                    Log.Logger.Error(sqlString + " -> " + ex.Message);
                    throw;
                }
                finally
                {
                    if (connection != null) connection.Close();
                }

                return true;
            });
        }

        static public bool ValidateParameter(string parameter)
        {
            return !string.IsNullOrWhiteSpace(parameter);
        }

        static public string? GetString(SqlDataReader r, string column)
        {
            return r.IsDBNull(r.GetOrdinal(column)) ? null : (string)r[column];
        }

        static public int GetId(SqlDataReader r, string column)
        {
            return (int)r[column];
        }

        static public int? GetIdNull(SqlDataReader r, string column)
        {
            return r.IsDBNull(r.GetOrdinal(column)) ? null : (int)r[column];
        }

        static public int GetInt(SqlDataReader r, string column)
        {
            return (int)r[column];
        }
        static public int? GetIntNull(SqlDataReader r, string column)
        {
            return r.IsDBNull(r.GetOrdinal(column)) ? null : (int)r[column];
        }

        static public bool GetBoolean(SqlDataReader r, string column)
        {
            return !r.IsDBNull(r.GetOrdinal(column)) && r.GetBoolean(r.GetOrdinal(column));
        }

        static public DateTime GetDateTime(SqlDataReader r, string column)
        {
            return r[column] == DBNull.Value ? DateTime.MinValue : (DateTime)r[column];
        }


    }
}
