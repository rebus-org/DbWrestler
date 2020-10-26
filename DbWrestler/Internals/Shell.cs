using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DbWrestler.Internals
{
    static class Shell
    {
        public static IReadOnlyList<string> Execute(string fileName, string arguments, int commandTimeoutSeconds)
        {
            using var process = new Process
            {
                StartInfo =
                {
                    FileName = fileName,
                    Arguments = arguments,

                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            };

            var outputLines = new List<string>();

            void AppendLine(string text, string prefix = null)
            {
                if (string.IsNullOrWhiteSpace(text)) return;

                outputLines.Add(prefix == null ? text : $"{prefix}: {text}");
            }

            process.OutputDataReceived += (o, ea) => AppendLine(ea.Data);
            process.ErrorDataReceived += (o, ea) => AppendLine(ea.Data, "ERROR");

            if (!process.Start())
            {
                throw new ShellExecuteException($@"Command

    {fileName} {arguments}

could not start process.

Captured output:

{string.Join(Environment.NewLine, outputLines)}");
            }

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            if (!process.WaitForExit((int)TimeSpan.FromSeconds(commandTimeoutSeconds).TotalMilliseconds))
            {
                throw new TimeoutException($@"Command

    {fileName} {arguments}

did not exit within {commandTimeoutSeconds} s timeout.

Captured output:

{string.Join(Environment.NewLine, outputLines)}");
            }

            if (process.ExitCode != 0)
            {
                throw new ShellExecuteException($@"Command

    {fileName} {arguments}

exited with nonzero exit code: {process.ExitCode}

Captured output:

{string.Join(Environment.NewLine, outputLines)}");
            }

            return outputLines.Where(line => line != null).ToList();
        }

    }
}