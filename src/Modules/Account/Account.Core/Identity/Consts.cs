namespace Account.Core.Identity
{
    internal static class Consts
    {
        internal static class PasswordHasher
        {
            internal const int SaltSize            = 16;
            internal const int HashSize            = 32;
            internal const int DegreeOfParallelism = 8;
            internal const int Iterations          = 4;
            internal const int MemorySize          = 65536;
        }
    }
}