using BloodDonation.Models;
using BloodDonation.Provider.Interfaces;
using BloodDonation.Repository.Interfaces;
using Dapper;

namespace BloodDonation.Repository;

public class UserRepository : IUserRepository
{
    private readonly IDbConnectionProvider _dbConnectionProvider;

    public UserRepository(IDbConnectionProvider dbConnectionProvider)
    {
        _dbConnectionProvider = dbConnectionProvider;
    }

    public async Task<List<User>> GetUsersByType(string type)
    {
        var conn = _dbConnectionProvider.GetConnection();
        return (await conn.QueryAsync<User>("SELECT * FROM user WHERE UserLevel = @type", new
        {
            type = type
        })).ToList();
    }
}