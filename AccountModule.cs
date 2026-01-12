using System;

public static class AccountModule
{
    public static IServiceCollection AddAccountModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services;
    }
}
