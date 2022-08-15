using BloodDonation.Provider;
using BloodDonation.Provider.Interfaces;

namespace BloodDonation;

public static class DiConfig
{
    public static IServiceCollection UseBloodDonation(this IServiceCollection services)
    {
        services.AddScoped<IDbConnectionProvider, DbConnectionProvider>();
        return services;
    }
}