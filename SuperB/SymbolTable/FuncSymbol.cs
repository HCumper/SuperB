namespace SuperB.SymbolTable
{
    public class FuncSymbol : Symbol
    {
        public FuncSymbol(string name, int type, string scope, int returnType)
            : base(name, type, scope)
        {
            ReturnType = returnType;
        }

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private int ReturnType { get; }
    }
}