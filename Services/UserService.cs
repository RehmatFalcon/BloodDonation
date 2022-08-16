using System.Transactions;
using BloodDonation.Crypter.Interfaces;
using BloodDonation.Dto;
using BloodDonation.Models;
using BloodDonation.Provider.Interfaces;
using BloodDonation.Services.Interfaces;
using Dapper;

namespace BloodDonation.Services;

public class UserService : IUserService
{
    private readonly IDbConnectionProvider _dbConnectionProvider;
    private readonly ICrypter _crypter;

    public UserService(IDbConnectionProvider dbConnectionProvider, ICrypter crypter)
    {
        _dbConnectionProvider = dbConnectionProvider;
        _crypter = crypter;
    }

    public async Task<User> Create(UserDto userDto)
    {
        using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        await using var conn = _dbConnectionProvider.GetConnection();
        var user = new User()
        {
            UserName = userDto.UserName,
            UserLevel = userDto.UserLevel,
            CreatedDate = DateTime.UtcNow,
            Password = _crypter.Hash(userDto.Password)
        };
        user.Id = await conn.ExecuteScalarAsync<long>(
            @"INSERT INTO blood.user (UserName, Password, CreatedDate, UserLevel)
VALUES (@UserName, @Password, @CreatedDate, @UserLevel);
select LAST_INSERT_ID();
        ", user);
        tx.Complete();
        return user;
    }
}