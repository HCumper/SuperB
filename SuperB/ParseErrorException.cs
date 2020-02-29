using System;

namespace SuperB
{
    public enum Severity { Error, Warning, Info }

    public class ParseErrorException : Exception
    {
        public ParseErrorException(string message)
            : base(message)
        {
            Severity = Severity.Error;
        }

        public Severity Severity { get; set; }
    }
}
