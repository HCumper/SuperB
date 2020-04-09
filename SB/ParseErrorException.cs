using System;
using System.Runtime.Serialization;

namespace SB
{
    public enum Severity { Error, Warning, Info }

    [Serializable]
    public class ParseErrorException : Exception
    {
        public ParseErrorException()
        {
        }

        public ParseErrorException(string message)
            : base(message)
        {
            Severity = Severity.Error;
        }

        public ParseErrorException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ParseErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public Severity Severity { get; set; }
    }
}
