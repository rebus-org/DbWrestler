using System;
using System.Linq;
using DbWrestler.Internals;

namespace DbWrestler
{
    public class DbInstance
    {
        readonly string _instanceName;
        readonly string _databaseName;

        public DbInstance(string instanceName, string databaseName)
        {
            _instanceName = instanceName;
            _databaseName = databaseName;
        }

        public void CreateIfNotExists()
        {
            if (Exists) return;

            Create();
        }

        public void DeleteIfExists()
        {
            if (!Exists) return;

            Delete();
        }

        public void Create()
        {
            using var connection = new ConnHelper(_instanceName).OpenConnection("master");

            connection.NonQuery($"CREATE DATABASE [{_databaseName}]");
        }

        public void Delete()
        {
            using var connection = new ConnHelper(_instanceName).OpenConnection("master");

            connection.NonQuery($"ALTER DATABASE [{_databaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;");

            connection.NonQuery($"DROP DATABASE [{_databaseName}]");
        }

        public bool Exists
        {
            get
            {
                using var connection = new ConnHelper(_instanceName).OpenConnection("master");

                var names = connection.Query("select [name] from [sys].[databases]").ToList();

                return names.Any(name => string.Equals(_databaseName, name, StringComparison.OrdinalIgnoreCase));
            }
        }

        public string ConnectionString => new ConnHelper(_instanceName).GetConnectionString(_databaseName);
    }
}