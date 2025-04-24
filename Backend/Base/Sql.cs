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
                    Console.WriteLine(ex.Message); //TODO - LogMe
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
                var connectionString = "Server=np:localhost;Database=blue;TrustServerCertificate=True;Authentication=Active Directory Integrated;";

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
                    Console.WriteLine(ex.Message); //TODO - LogMe
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


    }
}
