using MySql.Data.MySqlClient;

namespace BloodDonation.Provider.Interfaces;

public interface IDbConnectionProvider
{
    MySqlConnection GetConnection();
}