using System;
using Antlr4.Runtime;

namespace SuperB
{
    internal class SuperBTokenFactory : ITokenFactory
    {
        public IToken Create(Tuple<ITokenSource, ICharStream> source, int type, string text, int channel, int start,
            int stop, int line, int charPositionInLine)
        {
            return new SuperBToken(source, type, channel, start, stop);
        }

        public IToken Create(int type, string text)
        {
            return new SuperBToken(type, text);
        }
    }
}