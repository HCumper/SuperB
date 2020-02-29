using System;
using System.Collections.Generic;
using System.Text;

namespace SB
{
    public enum Severity { Error, Warning, Info }

    [Serializable]
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
