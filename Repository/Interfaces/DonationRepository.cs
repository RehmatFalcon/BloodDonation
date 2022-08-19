using BloodDonation.Constants;
using BloodDonation.Models;
using BloodDonation.Provider.Interfaces;
using BloodDonation.Results;
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

    public async Task<IEnumerable<Donation>> GetRecentDonations(int donorId, int count = 5)
    {
        await using var conn = _connectionProvider.GetConnection();
        return await conn.QueryAsync<Donation>("SELECT * from donation where UserDetailsId = @donorId LIMIT @count", new
        {
            count = count,
            donorId = donorId
        });
    }

    public async Task<PagedResult<Donation>> GetDonations(DateTime from, DateTime to, int page, int limit)
    {
        await using var conn = _connectionProvider.GetConnection();
        var query = "SELECT * FROM donation where date(DATE) BETWEEN date(@from) and date(@to) and status = @status";
        var countQuery = query.Replace("SELECT * ", "SELECT COUNT(*) ");
        query += " limit @offset, @limit";
        var p = new
        {
            from = from.Date,
            to = to.Date,
            status = DonationStatus.Verified,
            offset = (page - 1) * limit,
            limit = limit,
        };
        var totalCount = await conn.ExecuteScalarAsync<int>(countQuery, p);
        var data = await conn.QueryAsync<Donation>(query, p);

        return new PagedResult<Donation>()
        {
            Collection = data.ToList(),
            TotalCount = totalCount,
            Limit = limit,
            Page = page,
        };
    }
}