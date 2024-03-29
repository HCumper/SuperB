﻿namespace SuperB
{
    public interface ISymbolTable
    {
        void AddSymbol(Symbol symbol);
        Symbol ReadSymbol(string name, string scope);
    }
}
