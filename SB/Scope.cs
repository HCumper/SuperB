using System.Collections.Generic;

namespace SuperB
{
    public class Scope
    {
        public string Name { get; set; }
        Dictionary<string, Symbol> Entries = new Dictionary<string, Symbol>();

        public Scope(string name) => Name = name;
        // INCOMPLETE
        public Symbol GetElement(Symbol symbol)
        {
            //Symbol foundSymbol = new Symbol();

            //bool found = Entries.TryGetValue(symbol.Name, out foundSymbol);
            //if (!found) return null;

            //if (symbol.Type != foundSymbol.Type)
            //{
            //    throw new ParseError("Name " + symbol.Name + " wrong type");
            //}

            //if (symbol.ArrayParameters == null) return foundSymbol;

            //// string slicing unsupported
            //if (symbol.ArrayParameters.Count != foundSymbol.ArrayParameters.Count)
            //{
            //    throw new ParseError("Name " + symbol.Name + " wrong number of parameters specified");
            //}

            //for (int i = 0; i < symbol.ArrayParameters.Count; i++)
            //{
            //    if (symbol.ArrayParameters[i] > foundSymbol.ArrayParameters[i])
            //    {
            //        throw new ExecutionError("Name " + symbol.Name + " index " + i + " out of bounds");
            //    }
            //}

            int subscript = 0;
            // create collection of multipliers for each dimension
            //arrayParameters.Aggregate((total, next) => total *= next); 
            return null;
        }

        public Symbol AddElement(Symbol symbol)
        {
            bool found = Entries.ContainsKey(symbol.Name);
            if (found)
            {
                return null;
            }
            else
            {
                Entries.Add(symbol.Name, symbol);
            }
            return symbol;
        }

        // symbol must be fully populated
        public Symbol UpdateElement(Symbol symbol)
        {
            bool found = Entries.ContainsKey(symbol.Name);
            if (!found)
            {
                return null;
            }
            else
            {
                Entries[symbol.Name] = symbol;
            }
            return symbol;
        }
    }
}


