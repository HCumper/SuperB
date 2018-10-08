using System;

namespace SuperB
{
    public enum Severities
    {
        Error,
        // ReSharper disable once UnusedMember.Global
        Warning,
        // ReSharper disable once UnusedMember.Global
        Info
    }

    public class ParseError : Exception
    {
        public ParseError(string message)
            : base(message)
        {
            Severity = Severities.Error;
        }

        public Severities Severity { get; set; }
    }
}