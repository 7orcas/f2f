using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.App
{
    public class Sql
    {

        static public async Task<bool> Run(string sqlString, Action<SqlDataReader> action)
        {
            return await Task.Run(() =>
            {
                
//Thread.Sleep(400);
                var connectionString = "Server=np:localhost;Database=dsp;TrustServerCertificate=True;Authentication=Active Directory Integrated;";

                SqlConnection connection = null;
                SqlDataReader reader = null;
                try
                {
                    connection = new SqlConnection(connectionString);
                    connection.Open();
                    var command = new SqlCommand(sqlString, connection);
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
                var connectionString = "Server=np:localhost;Database=dsp;TrustServerCertificate=True;Authentication=Active Directory Integrated;";

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

        static public string AddBaseEntity (string table)
        {
            return AddBaseEntity(table, "b");
        }

        static public string AddBaseEntity(string table, string baseTable)
        {
            return " INNER JOIN App.BaseEntity " + baseTable + " ON " + baseTable + ".Id = " + table + "._id";
        }

    }
}
