using System.Transactions;
using BloodDonation.Crypter.Interfaces;
using BloodDonation.Dto;
using BloodDonation.Models;
using BloodDonation.Provider.Interfaces;
using BloodDonation.Repository.Interfaces;
using BloodDonation.Services.Interfaces;
using Dapper;

namespace BloodDonation.Services;

public class UserService : IUserService
{
    private readonly IDbConnectionProvider _dbConnectionProvider;
    private readonly ICrypter _crypter;
    private readonly IUserRepository _userRepo;

    public UserService(IDbConnectionProvider dbConnectionProvider, ICrypter crypter, IUserRepository userRepo)
    {
        _dbConnectionProvider = dbConnectionProvider;
        _crypter = crypter;
        _userRepo = userRepo;
    }

    public async Task<User> Create(UserDto userDto)
    {
        using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        await using var conn = _dbConnectionProvider.GetConnection();
        await Validate(userDto.UserName);
        var user = new User()
        {
            UserName = userDto.UserName,
            UserLevel = userDto.UserLevel,
            CreatedDate = DateTime.UtcNow,
            Password = _crypter.Hash(userDto.Password)
        };
        user.Id = await conn.ExecuteScalarAsync<long>(
            @"INSERT INTO user (UserName, Password, CreatedDate, UserLevel)
VALUES (@UserName, @Password, @CreatedDate, @UserLevel);
select LAST_INSERT_ID();
        ", user);
        tx.Complete();
        return user;
    }

    public async Task ResetPassword(long id, string newPassword)
    {
        using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        await using var conn = _dbConnectionProvider.GetConnection();
        await conn.ExecuteAsync("UPDATE user set Password = @password where Id = @id", new
        {
            password = _crypter.Hash(newPassword),
            id = id
        });
        tx.Complete();
    }

    public async Task ChangePassword(User user, string oldPassword, string newPassword)
    {
        using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        if (!_crypter.Verify( oldPassword, user.Password))
        {
            throw new Exception("Wrong password");
        }

        await ResetPassword(user.Id, newPassword);
        tx.Complete();
    }

    private async Task Validate(string userName)
    {
        if (await _userRepo.GetByUserName(userName) != null)
        {
            throw new Exception($"Username `{userName}` already taken. Please use another username");
        }
    }
}