using Antlr4.Runtime.Tree;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SuperB
{
    public class Symbol
    {
        public string Name { get; set; }
        public int Type { get; set; }
        public string Scope { get; set; }

        public Symbol()
        { }

        public Symbol(string name, int type, string scope)
        {
            this.Name = name;
            this.Type = type;
            this.Scope = scope;
        }
    }

    public class ParamSymbol : Symbol
    {
        bool Reference { get; set; }
        public ParamSymbol(string name, int type, string scope, bool reference)
            : base(name, type, scope)
        {
            this.Reference = reference;
        }
    }

    public class FuncSymbol : Symbol
        {
        int ReturnType { get; set; }
        public FuncSymbol(string name, int type, string scope, int returnType)
            : base(name, type, scope)
        {
            this.ReturnType = returnType;
        }
    }

    public class ArraySymbol : Symbol
    {
        List<int> Dimensions { get; set; }
        public ArraySymbol (string name, int type, string scope, List<int> dimensions)
            : base(name, type, scope)
        {
            this.Dimensions = dimensions;
        }
    }
}
