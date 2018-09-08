using System;
using System.Collections.Generic;
using System.Text;

namespace SuperB
{
    public enum Severities { Error, Warning, Info }

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
