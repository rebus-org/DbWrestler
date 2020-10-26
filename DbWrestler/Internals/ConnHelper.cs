using Microsoft.Data.SqlClient;

namespace DbWrestler.Internals
{
    class ConnHelper
    {
        readonly string _instanceName;

        public ConnHelper(string instanceName)
        {
            _instanceName = instanceName;
        }

        public SqlConnection OpenConnection(string databaseName)
        {
            var connectionString = GetConnectionString(databaseName);

            var sqlConnection = new SqlConnection(connectionString);

            try
            {
                sqlConnection.Open();

                return sqlConnection;
            }
            catch
            {
                sqlConnection.Dispose();
                throw;
            }
        }

        public string GetConnectionString(string databaseName) => $"server=(localdb)\\{_instanceName}; database={databaseName}; trusted_connection=true";
    }
}