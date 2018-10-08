using System;
using System.Collections.Generic;

namespace SuperB.SymbolTable
{
    // Compile time structure for storing information about symbols. Single monolithic store keyed by name and scope
    public class SymbolTable : ISymbolTable
    {
        private readonly IDictionary<Tuple<string, string>, Symbol> _table =
            new Dictionary<Tuple<string, string>, Symbol>();

        public void AddSymbol(Symbol symbol)
        {
            if (!_table.ContainsKey(Tuple.Create(symbol.Name, symbol.Scope)))
                _table.Add(new KeyValuePair<Tuple<string, string>, Symbol>(Tuple.Create(symbol.Name, symbol.Scope),
                    symbol));
        }

        public Symbol ReadSymbol(string name, string scope)
        {
            if (!_table.ContainsKey(Tuple.Create(name, scope))) return null;
            return _table[Tuple.Create(name, scope)];
        }

        public Symbol ReadAnySymbol(string name, string scope)
        {
            if (_table.ContainsKey(Tuple.Create(name, scope))) 
            return _table[Tuple.Create(name, scope)];
            else
            {
                if (_table.ContainsKey(Tuple.Create(name, "~GLOBAL")))
                    return _table[Tuple.Create(name, "~GLOBAL")];
                else return null;
            }
        }

    }
}