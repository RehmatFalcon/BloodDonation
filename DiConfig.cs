using BloodDonation.Crypter.Interfaces;
using BloodDonation.Provider;
using BloodDonation.Provider.Interfaces;
using BloodDonation.Repository;
using BloodDonation.Repository.Interfaces;
using BloodDonation.Seeder;
using BloodDonation.Seeder.Interfaces;
using BloodDonation.Services;
using BloodDonation.Services.Interfaces;

namespace BloodDonation;

public static class DiConfig
{
    public static IServiceCollection UseBloodDonation(this IServiceCollection services)
    {
        UseRepo(services);
        UseServices(services);
        UseMisc(services);
        return services;
    }

    private static void UseRepo(IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
    }

    private static void UseServices(IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
    }

    private static void UseMisc(IServiceCollection services)
    {
        services.AddScoped<IDbConnectionProvider, DbConnectionProvider>()
            .AddScoped<ICrypter, Crypter.Crypter>()
            .AddScoped<IUserSeeder, UserSeeder>();
    }
}