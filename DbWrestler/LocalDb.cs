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

        public LocalDbInstance GetInstance(string instanceName = DefaultInstanceName) => new(instanceName, _exePath, _commandTimeoutSeconds);

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

            var directories = new[]
            {
                Environment.ExpandEnvironmentVariables(@"%ProgramFiles%\Microsoft SQL Server\130\Tools\Binn")
            };

            foreach (var directory in directories)
            {
                var exePath = Path.Combine(directory, "SqlLocalDB.exe");

                if (File.Exists(exePath)) return exePath;
            }

            throw new InvalidOperationException($@"Could not find SQL Local DB executable - looked for:

    SqlLocalDB.exe

in these locations:

{string.Join(Environment.NewLine, directories.Select(d => $"    {d}"))}

Is LocalDB installed on this machine? ({Environment.MachineName})");
        }
    }
}
