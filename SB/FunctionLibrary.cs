namespace SuperB
{
    // Implements SuperB keywords that are just function calls
    internal static class FunctionLibrary
    {
        public static float Abs(float param)
        {
            return (param >= 0) ? param : param * -1;
        }
    }
}
