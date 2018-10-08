//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace SuperB.SymbolTable
//{
//    // run time structure for storing current values. 1 global space and a stack of local ones
//    public class MemorySpace
//    {
//        private readonly Scope _globalScope = new Scope("GLOBAL");
//        private readonly ISet<Scope> _localScopes = new HashSet<Scope>();

//        private MemorySpace()
//        {
//            // Prime the global table with function style keywords
//        }

//        public Symbol AddSymbol(Symbol symbol)
//        {
//            throw new NotImplementedException();
//        }

//        public Symbol UpdateSymbol(Symbol symbol)
//        {
//            throw new NotImplementedException();
//        }

//        private Symbol AddOrUpdateSymbol(Symbol symbol)
//        {
//            var tableSymbol = UpdateSymbol(symbol) ?? AddSymbol(symbol);
//            return tableSymbol;
//        }

//        private void CreateLocalScope(string name)
//        {
//            var newScope = new Scope(name);
//            _localScopes.Add(newScope);
//        }

//        private Symbol ReadSymbol(Symbol symbol, Scope scope)
//        {
//            Symbol foundSymbol = null;
//            if (_localScopes.Count != 0)
//            {
//                var current = _localScopes.FirstOrDefault(w => w.Name.Equals("scope"));
//                if (current != null) foundSymbol = current.GetElement(symbol);
//            }

//            return foundSymbol ?? (_globalScope.GetElement(symbol));
//        }

//        private int FindSubscript(IList<int> subscripts, IList<int> dimensions)
//        {
//            if (subscripts.Count != dimensions.Count) throw new ParseError("Wrong number of subscripts");
//            for (var i = 0; i < subscripts.Count; i++)
//                if (subscripts[i] > dimensions[i])
//                    throw new ParseError(
//                        $"Subscript {i + 1} is {subscripts[i]} which too large for dimension {dimensions[i]}");
//            for (var i = 0; i < subscripts.Count; i++) subscripts[i]--;
//            int oneDSubscript = 0, dimensionSize = 1;
//            while (subscripts.Count > 0)
//            {
//                oneDSubscript += subscripts[subscripts.Count - 1] * dimensionSize;
//                subscripts.RemoveAt(subscripts.Count - 1);
//                dimensionSize *= dimensions[subscripts.Count];
//            }

//            return oneDSubscript;
//        }
//    }
//}