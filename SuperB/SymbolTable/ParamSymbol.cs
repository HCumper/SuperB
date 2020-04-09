namespace SuperB.SymbolTable
{
    public class ParamSymbol : Symbol
    {
        public ParamSymbol(string name, int type, string scope, bool reference)
            : base(name, type, scope)
        {
            Reference = reference;
        }

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private bool Reference { get; }
    }
}