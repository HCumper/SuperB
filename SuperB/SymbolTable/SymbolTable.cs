using System;
using System.Collections.Generic;
using System.Linq;

namespace SuperB
{
    // Compile time structure for storing information about symbols. Single monolithic store keyed by name and scope
    public class SymbolTable : ISymbolTable
    {
        private IDictionary<Tuple<string, string>, Symbol> Table = new Dictionary<Tuple<string, string>, Symbol>();

        public SymbolTable()
        {
            //this.AddSymbol(new Symbol("ABS", SBLexer.Real, "~GLOBAL"));
            //this.AddSymbol(new Symbol("Param1", SBLexer.Real, "ABS"));
            //this.AddSymbol(new Symbol("ACOS", SBLexer.Real, "~GLOBAL"));
            //this.AddSymbol(new Symbol("Param1", SBLexer.Real, "ACOS"));
            //this.AddSymbol(new Symbol("ASIN", SBLexer.Real, "~GLOBAL"));
            //this.AddSymbol(new Symbol("Param1", SBLexer.Real, "~ASIN"));
            //this.AddSymbol(new Symbol("ACOT", SBLexer.Real, "~GLOBAL"));
            //this.AddSymbol(new Symbol("Param1", SBLexer.Real, "ACOT"));
            //this.AddSymbol(new Symbol("ATAN", SBLexer.Real, "~GLOBAL"));
            //this.AddSymbol(new Symbol("Param1", SBLexer.Real, "ATAN"));
            //this.AddSymbol(new Symbol("ADATE", SBLexer.Void, "~GLOBAL"));
            //this.AddSymbol(new Symbol("Param1", SBLexer.Real, "~GLOBAL"));
            //this.AddSymbol(new Symbol("ARC", SBLexer.Real, "~GLOBAL"));
            //this.AddSymbol(new Symbol("ARC_R", SBLexer.Real, "~GLOBAL"));

        }

        public void AddSymbol(Symbol symbol)
        {
            if (!Table.ContainsKey(Tuple.Create(symbol.Name, symbol.Scope))) Table.Add(new KeyValuePair<Tuple<string, string>, Symbol>(Tuple.Create(symbol.Name, symbol.Scope), symbol));
        }

        public Symbol ReadSymbol(string name, string scope)
        {
            if (!Table.ContainsKey(Tuple.Create(name, scope))) return null;
            else
                return Table[Tuple.Create(name, scope)];
        }
    }
}
