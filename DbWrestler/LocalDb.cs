using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DbWrestler.Internals;

namespace DbWrestler
{
    public class LocalDb
    {
        public const string DefaultInstanceName = "MSSQLLocalDB";

        readonly int _commandTimeoutSeconds;
        readonly string _exePath;

        public LocalDb(int commandTimeoutSeconds = 60)
        {
            _commandTimeoutSeconds = commandTimeoutSeconds;
            _exePath = GetLocalDbExecutablePath();
        }

        public LocalDbInstance GetInstance(string instanceName = DefaultInstanceName) => new LocalDbInstance(instanceName, _exePath, _commandTimeoutSeconds);

        public IReadOnlyList<string> GetInstanceNames()
        {
            return Shell.Execute(_exePath, "info", _commandTimeoutSeconds)
                .Select(line => line.Trim())
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .ToList();
        }

        static string GetLocalDbExecutablePath()
        {
            // C:\Program Files\Microsoft SQL Server\130\Tools\Binn

            var directory = Environment.ExpandEnvironmentVariables(@"%ProgramFiles%\Microsoft SQL Server\130\Tools\Binn");

            var exePath = Path.Combine(directory, "SqlLocalDB.exe");

            if (!File.Exists(exePath))
            {
                throw new InvalidOperationException($@"Could not find SQL Local DB executable here:

    {exePath}

Is LocalDB even installed on this machine? ({Environment.MachineName})");
            }

            return exePath;
        }
    }
}
