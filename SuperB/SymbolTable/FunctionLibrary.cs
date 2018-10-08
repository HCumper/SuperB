namespace SuperB.SymbolTable
{
    // Implements SuperB keywords that are just function calls
    // ReSharper disable once UnusedMember.Global
    internal class FunctionLibrary
    {
        // ReSharper disable once UnusedMember.Global
        public float Abs(float param)
        {
            return param >= 0 ? param : param * -1;
        }
    }
}