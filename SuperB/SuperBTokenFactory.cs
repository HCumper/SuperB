using Antlr4.Runtime;
using System;

namespace SuperB
{
    internal class SuperBTokenFactory : ITokenFactory
    {
        public IToken Create(Tuple<ITokenSource, ICharStream> source, int type, string text, int channel, int start, int stop, int line, int charPositionInLine)
        {
            return new SuperBToken(source, type, text, channel, start, stop, line, charPositionInLine);
        }

        public IToken Create(int type, string text)
        {
            return new SuperBToken(type, text);
        }
    }

    public class SuperBToken : CommonToken
    {
        public int EvaluatedType { get; set; } // SuperBLexer type

        public SuperBToken(int type) : base(type)
        {
        }

        public SuperBToken(int type, string text) : base(type, text)
        {
        }

        public SuperBToken(Tuple<ITokenSource, ICharStream> source, int type, string text, int channel, int start, int stop, int line, int charPositionInLine)
            : base(source, type, channel, start, stop)
        { }
    }
}