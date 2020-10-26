using System;

namespace DbWrestler
{
    public class ShellExecuteException : Exception
    {
        public ShellExecuteException(string message) : base(message)
        {
        }
    }
}