using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace DbWrestler.Internals
{
    static class ConnectionExtensions
    {
        public static void NonQuery(this SqlConnection connection, string sql)
        {
            using var command = connection.CreateCommand();

            command.CommandText = sql;

            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                throw new ArgumentException($@"Could not execute the following SQL:

{sql}", exception);
            }
        }

        public static IEnumerable<string> Query(this SqlConnection connection, string sql)
        {
            using var command = connection.CreateCommand();

            command.CommandText = sql;

            SqlDataReader ExecuteReader()
            {
                try
                {
                    return command.ExecuteReader();
                }
                catch (Exception exception)
                {
                    throw new ArgumentException($@"Could not execute the following SQL:

{sql}", exception);
                }
            }

            using var reader = ExecuteReader();

            if (reader.FieldCount != 1)
                throw new ArgumentException($@"Query SQL 

{sql}

returned more than one field per row");

            var name = reader.GetName(0);

            while (reader.Read())
            {
                yield return (string)reader[name];
            }
        }
    }
}