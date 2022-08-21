using System.Transactions;
using BloodDonation.Constants;
using BloodDonation.Dto;
using BloodDonation.Models;
using BloodDonation.Provider.Interfaces;
using BloodDonation.Services.Interfaces;
using Dapper;

namespace BloodDonation.Services;

public class DonationService : IDonationService
{
    private readonly IDbConnectionProvider _connectionProvider;

    public DonationService(IDbConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public async Task<Donation> Create(DonationCreateDto dto)
    {
        using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        await using var conn = _connectionProvider.GetConnection();
        var donation = new Donation()
        {
            Date = dto.Date,
            Name = dto.Name,
            Receiver = dto.Receiver,
            Status = dto.Status,
            Type = dto.Type,
            BloodGroup = dto.BloodGroup,
            ContactNo = dto.ContactNo,
            DonationDistrict = dto.DonationDistrict,
            DonationLocation = dto.DonationLocation,
            UserDetailsId = dto.UserDetailsId
        };
        donation.Id = await conn.ExecuteScalarAsync<int>(
            @"INSERT INTO blood.donation (UserDetailsId, Date, Name, BloodGroup, ContactNo, DonationDistrict, DonationLocation, Receiver,
                            Type, Status)
VALUES (@UserDetailsId, @Date, @Name, @BloodGroup, @ContactNo, @DonationDistrict, @DonationLocation, @Receiver, @Type, @Status);
SELECT LAST_INSERT_ID();
", donation);
        await HandleDonationRecordSnapshot(donation);
        tx.Complete();
        return donation;
    }

    public async Task Approve(Donation donation)
    {
        using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        if (donation.Status == DonationStatus.Verified) throw new Exception("Record already verified");
        await using var conn = _connectionProvider.GetConnection();
        donation.Status = DonationStatus.Verified;
        await conn.ExecuteScalarAsync("UPDATE donation SET Status = @status where Id = @Id", new
        {
            Id = donation.Id,
            status = donation.Status
        });
        await HandleDonationRecordSnapshot(donation);
        tx.Complete();
    }

    public async Task Delete(Donation donation)
    {
        using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        if (donation.Status == DonationStatus.Verified) throw new Exception("Record already verified");
        await using var conn = _connectionProvider.GetConnection();
        await conn.ExecuteAsync("DELETE donation where Id = @Id", new
        {
            Id = donation.Id
        });
        tx.Complete();
    }

    private async Task HandleDonationRecordSnapshot(Donation donation)
    {
        if (donation.IsVerified() && donation.IsDonation() && donation.UserDetailsId.HasValue)
        {
            await using var conn = _connectionProvider.GetConnection();
            await conn.ExecuteScalarAsync(
                "UPDATE user_details set DonationCount = DonationCount + 1, LastDonationDate = IF(date(@DonationDate) > date (LastDonationDate), @DonationDate, LastDonationDate) where Id = @Id",
                new
                {
                    Id = donation.UserDetailsId,
                    DonationDate = donation.Date.Date
                });
        }
    }
}