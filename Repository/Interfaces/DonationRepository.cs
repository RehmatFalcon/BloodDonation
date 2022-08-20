using BloodDonation.Constants;
using BloodDonation.Models;
using BloodDonation.Provider.Interfaces;
using BloodDonation.Results;
using BloodDonation.ViewModels;
using Dapper;

namespace BloodDonation.Repository.Interfaces;

public class DonationRepository : IDonationRepository
{
    private readonly IDbConnectionProvider _connectionProvider;

    public DonationRepository(IDbConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public async Task<Donation?> Find(long id)
    {
        await using var conn = _connectionProvider.GetConnection();
        return await conn.QueryFirstOrDefaultAsync<Donation>("SELECT * FROM donation where Id = @Id", new
        {
            Id = id
        });
    }

    public async Task<IEnumerable<Donation>> GetRequests()
    {
        await using var conn = _connectionProvider.GetConnection();
        return await conn.QueryAsync<Donation>("SELECT * from donation where Status = @status", new
        {
            status = DonationStatus.Unverified
        });
    }

    public async Task<IEnumerable<Donation>> GetRecentDonations(long donorId, int count = 5)
    {
        await using var conn = _connectionProvider.GetConnection();
        return await conn.QueryAsync<Donation>("SELECT * from donation where UserDetailsId = @donorId LIMIT @count", new
        {
            count = count,
            donorId = donorId
        });
    }

    public async Task<PagedResult<Donation>> GetDonations(DonationSearchVm vm, long? donorId)
    {
        await using var conn = _connectionProvider.GetConnection();
        var query = "SELECT * FROM donation where date(DATE) BETWEEN date(@From) and date(@To)";

        if (!string.IsNullOrEmpty(vm.Status))
        {
            query += " and Status = @Status";
        }

        if (!string.IsNullOrEmpty(vm.Name))
        {
            query += " and Name like '%@Name%'";
        }

        if (!string.IsNullOrEmpty(vm.Receiver))
        {
            query += " and Receiver like '%@Receiver%'";
        }

        if (!string.IsNullOrEmpty(vm.DonationDistrict))
        {
            query += " and DonationDistrict = @DonationDistrict";
        }

        if (!string.IsNullOrEmpty(vm.Type))
        {
            query += " and Type = @Type";
        }

        if (!string.IsNullOrEmpty(vm.DonationLocation))
        {
            query += " and DonationLocation like '%@DonationLocation%'";
        }

        if (!string.IsNullOrEmpty(vm.BloodGroup))
        {
            query += " and BloodGroup = @BloodGroup";
        }

        if (donorId.HasValue)
        {
            query += " and UserDetailsId = @DonorId";
        }


        var countQuery = query.Replace("SELECT * ", "SELECT COUNT(*) ");
        query += " limit @offset, @limit";
        var p = new
        {
            From = vm.From.Date,
            To = vm.To.Date,
            Status = vm.Status,
            Name = vm.Name,
            Receiver = vm.Receiver,
            DonationDistrict = vm.DonationDistrict,
            Type = vm.Type,
            DonationLocation = vm.DonationLocation,
            BloodGroup = vm.BloodGroup,
            DonorId = donorId,
            offset = (vm.Page - 1) * vm.Limit,
            limit = vm.Limit,
        };
        var totalCount = await conn.ExecuteScalarAsync<int>(countQuery, p);
        var data = await conn.QueryAsync<Donation>(query, p);

        return new PagedResult<Donation>()
        {
            Collection = data.ToList(),
            TotalCount = totalCount,
            Limit = vm.Limit,
            Page = vm.Page,
        };
    }
}