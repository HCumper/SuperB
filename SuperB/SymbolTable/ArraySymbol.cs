using System.Collections.Generic;

namespace SuperB.SymbolTable
{
    public class ArraySymbol : Symbol
    {
        public ArraySymbol(string name, int type, string scope, List<int> dimensions)
            : base(name, type, scope)
        {
            Dimensions = dimensions;
        }

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private List<int> Dimensions { get; }
    }
}