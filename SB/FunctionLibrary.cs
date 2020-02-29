namespace SuperB
{
    // Implements SuperB keywords that are just function calls
    class FunctionLibrary
    {
        public float Abs(float param)
        {
            return (param >= 0) ? param : param * -1;
        }
    }
}
