using System;
using System.Collections.Generic;
using System.Linq;
using DbWrestler.Internals;

namespace DbWrestler
{
    public class LocalDbInstance
    {
        readonly string _instanceName;
        readonly string _exePath;
        readonly int _commandTimeoutSeconds;

        internal LocalDbInstance(string instanceName, string exePath, int commandTimeoutSeconds)
        {
            _instanceName = instanceName ?? throw new ArgumentNullException(nameof(instanceName));
            _exePath = exePath ?? throw new ArgumentNullException(nameof(exePath));
            _commandTimeoutSeconds = commandTimeoutSeconds;
        }

        public void CreateIfNotExists()
        {
            if (Exists) return;

            Create();
        }

        public void Create() => Execute($"create \"{_instanceName}\"");

        public void Delete() => Execute($"delete \"{_instanceName}\"");

        public bool Exists
        {
            get
            {
                var output = Execute("info");

                return output.Any(line =>
                    string.Equals(line.Trim(), _instanceName, StringComparison.OrdinalIgnoreCase));
            }
        }

        IReadOnlyList<string> Execute(string command) => Shell.Execute(_exePath, command, _commandTimeoutSeconds);

        public override string ToString() => $"LOCALDB: \"{_instanceName}\"";
    }
}