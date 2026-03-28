namespace Account.Core.Security
{
    internal static class Consts
    {
        //DO OBGADANIA
        internal const int SaltSize = 16;
        internal const int HashSize = 32;
        internal const int DegreeOfParallelism = 8;
        internal const int Iterations = 4;
        internal const int MemorySize = 1024 * 1024;
    }
}
