namespace SuperB.SymbolTable
{
    public class Symbol
    {
        public Symbol(string name, int type, string scope)
        {
            Name = name;
            Type = type;
            Scope = scope;
        }

        public string Name { get; set; }
        public int Type { get; set; }
        public string Scope { get; set; }
    }
}