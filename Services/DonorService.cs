using System.Transactions;
using BloodDonation.Constants;
using BloodDonation.Dto;
using BloodDonation.Models;
using BloodDonation.Provider.Interfaces;
using BloodDonation.Services.Interfaces;
using Dapper;

namespace BloodDonation.Services;

public class DonorService : IDonorService
{
    private readonly IUserService _userService;
    private readonly IDbConnectionProvider _dbConnectionProvider;

    public DonorService(IUserService userService, IDbConnectionProvider dbConnectionProvider)
    {
        _userService = userService;
        _dbConnectionProvider = dbConnectionProvider;
    }

    public async Task<UserDetails> Create(DonorCreateDto dto)
    {
        using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        var user = await _userService.Create(new UserDto()
        {
            UserName = dto.UserName,
            Password = dto.Password,
            UserLevel = UserLevels.NormalUser
        });
        var userDetails = new UserDetails()
        {
            UserId = user.Id,
            Name = dto.Name,
            Address = dto.Address,
            District = dto.District,
            Note = dto.Note,
            ContactNo = dto.ContactNo,
            BloodGroup = dto.BloodGroup,
            LastDonationDate = dto.LastDonationDate,
            DonationCount = dto.DonationCount
        };
        await using var conn = _dbConnectionProvider.GetConnection();
        userDetails.Id = await conn.ExecuteScalarAsync<long>(@"
INSERT INTO user_details (UserId, Name, District, ContactNo, BloodGroup, Address, Note, LastDonationDate, DonationCount)
VALUES (@UserId, @Name, @District, @ContactNo, @BloodGroup, @Address, @Note, @LastDonationDate, @DonationCount);
select LAST_INSERT_ID();
", userDetails);
        tx.Complete();
        return userDetails;
    }

    public async Task Update(long userDetailsId, DonorUpdateDto dto)
    {
        using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        await using var conn = _dbConnectionProvider.GetConnection();
        var userDetails = new UserDetails()
        {
            Id = userDetailsId,
            Name = dto.Name,
            Address = dto.Address,
            District = dto.District,
            Note = dto.Note,
            ContactNo = dto.ContactNo,
            BloodGroup = dto.BloodGroup
        };
        await conn.ExecuteAsync(
            @"UPDATE user_details set Name = @Name, District = @District, Address = @Address, BloodGroup = @BloodGroup, ContactNo = @ContactNo, Note = @Note
    where Id = @Id
", userDetails);
        tx.Complete();
    }
}