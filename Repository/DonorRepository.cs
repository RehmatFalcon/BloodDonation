using BloodDonation.Models;
using BloodDonation.Provider.Interfaces;
using BloodDonation.Repository.Interfaces;
using BloodDonation.Results;
using BloodDonation.ViewModels;
using Dapper;

namespace BloodDonation.Repository;

public class DonorRepository : IDonorRepository
{
    private readonly IDbConnectionProvider _connectionProvider;

    public DonorRepository(IDbConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public async Task<PagedResult<UserDetails>> SearchDonors(DonorSearchVm vm)
    {
        await using var conn = _connectionProvider.GetConnection();
        var query = @"SELECT * FROM user_details where true";
        if (vm.ShowEligibleOnly)
        {
            query += " and (LastDonationDate is null or date(LastDonationDate) <= date(@lastAllowedDonationDate))";
        }

        if (!string.IsNullOrEmpty(vm.Name))
        {
            query += " and lower(Name) like '%@Name%'";
        }

        if (!string.IsNullOrEmpty(vm.Address))
        {
            query += " and lower(Address) like '%@Address%'";
        }

        if (!string.IsNullOrEmpty(vm.District))
        {
            query += " and District = @District";
        }

        if (!string.IsNullOrEmpty(vm.BloodGroup))
        {
            query += " and BloodGroup = @BloodGroup";
        }

        if (!string.IsNullOrEmpty(vm.ContactNo))
        {
            query += " and lower(ContactNo) like '%@ContactNo%'";
        }

        var countQuery = query.Replace("SELECT *", "SELECT count(*)");
        var p = new
        {
            Name = vm.Name,
            Address = vm.Address,
            District = vm.District,
            BloodGroup = vm.BloodGroup,
            ContactNo = vm.ContactNo,
            lastAllowedDonationDate = DateTime.UtcNow.AddDays(-90),
            Offset = vm.Page - 1 * vm.Limit,
            Limit = vm.Limit
        };
        var count = await conn.ExecuteScalarAsync<long>(countQuery, p);

        query += " limit @Offset, @Limit";
        var data = (await conn.QueryAsync<UserDetails>(query, p)).ToList();
        return new PagedResult<UserDetails>()
        {
            Collection = data,
            Limit = vm.Limit,
            Page = vm.Page,
            TotalCount = count
        };
    }
}