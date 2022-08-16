using BloodDonation.Provider.Interfaces;
using MySql.Data.MySqlClient;

namespace BloodDonation.Provider;

public class DbConnectionProvider : IDbConnectionProvider
{
    private readonly IConfiguration _configuration;

    public DbConnectionProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public MySqlConnection GetConnection() => new MySqlConnection(_configuration.GetConnectionString("Default"));
}