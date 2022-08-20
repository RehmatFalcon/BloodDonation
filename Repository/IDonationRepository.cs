using BloodDonation.Models;
using BloodDonation.Results;

namespace BloodDonation.Repository;

public interface IDonationRepository
{
    Task<Donation?> Find(long id);
    Task<IEnumerable<Donation>> GetRequests();
    Task<IEnumerable<Donation>> GetRecentDonations(long donorId, int count = 5);
    Task<PagedResult<Donation>> GetDonations(DateTime from, DateTime to, int page, int limit);
}