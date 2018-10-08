using System;
using Antlr4.Runtime;

namespace SuperB
{
    public class SuperBToken : CommonToken
    {
        public SuperBToken(int type, string text) : base(type, text)
        {
        }

        public SuperBToken(Tuple<ITokenSource, ICharStream> source, int type, int channel, int start,
            int stop)
            : base(source, type, channel, start, stop)
        {
        }

        public int EvaluatedType { get; set; } // SuperBLexer type
    }
}