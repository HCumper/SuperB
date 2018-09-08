﻿using SuperB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SuperB
{
    // run time structure for storing current values. 1 global space and a stack of local ones
    public class MemorySpace
    {
        private Scope GlobalScope = new Scope("GLOBAL");
        private ISet<Scope> LocalScopes = new HashSet<Scope>();
        MemorySpace()
        {
            // Prime the global table with function style keywords
        }

        public Symbol AddSymbol(Symbol symbol)
        {
            throw new NotImplementedException();
        }

        public Symbol UpdateSymbol(Symbol symbol)
        {
            throw new NotImplementedException();
        }

        Symbol AddOrUpdateSymbol(Symbol symbol)
        {
            Symbol tableSymbol = UpdateSymbol(symbol);
            if (tableSymbol == null) tableSymbol = AddSymbol(symbol);
            return tableSymbol;
        }

        void CreateLocalScope(string name)
        {
            Scope newScope = new Scope(name);
            LocalScopes.Add(newScope);
        }

        Symbol ReadSymbol(Symbol symbol, Scope scope)
        {
            Symbol foundSymbol = null;
            if (LocalScopes.Count != 0)
            {
                Scope current = LocalScopes.Where(w => w.Name.Equals("scope")).FirstOrDefault();
                foundSymbol = current.GetElement(symbol);
            }
            if (foundSymbol == null)
            {
                foundSymbol = GlobalScope.GetElement(symbol);
            }
            return foundSymbol;
        }

        private int FindSubscript(IList<int> subscripts, IList<int>dimensions)
        {
            if (subscripts.Count() != dimensions.Count) throw new ParseError("Wrong number of subscripts");
            for (int i = 0; i < subscripts.Count(); i++)
            {
                if (subscripts[i] > dimensions[i]) throw new ParseError($"Subscript {i + 1} is {subscripts[i]} which too large for dimension {dimensions[i]}");
            }
            for (int i = 0; i < subscripts.Count(); i++) subscripts[i]--;
            int oneDSubscript = 0, dimensionSize = 1;
            while (subscripts.Count > 0)
            {
                oneDSubscript += subscripts[subscripts.Count - 1] * dimensionSize;
                subscripts.RemoveAt(subscripts.Count() - 1);
                dimensionSize *= dimensions[subscripts.Count()];
            }

            return oneDSubscript;
        }
    }
}
