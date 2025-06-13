using Microsoft.Data.SqlClient;
using System.Reflection.Metadata;

/// <summary>
/// Database access
/// Created: March 2025
/// [*Licence*]
/// Author: John Stewart
/// </summary>

namespace Backend.Base.Database
{
    public partial class Sql
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
    }
}
