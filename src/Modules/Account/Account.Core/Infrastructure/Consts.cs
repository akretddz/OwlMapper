namespace Account.Core.Infrastructure
{
    internal static class Consts
    {
        internal static class Tables
        {
            internal static class Names
            {
                internal const string Accounts = "Accounts";
                internal const string AccountIdentities = "AccountIdentities";
                internal const string AccountRoles = "AccountRoles";
                internal const string AccountSecrets = "AccountSecrets";
                internal const string AccountTokens = "AccountTokens";
                internal const string Identities = "Identities";
                internal const string Roles = "Roles";
            }
        }

        internal const string Schema = "account";

        internal static class Validations
        {
            internal const int EmailAddressMaxLength = 255;
            internal const int UsernameMaxLength = 50;
        }
    }
}