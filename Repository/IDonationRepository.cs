using BloodDonation.Models;
using BloodDonation.Results;
using BloodDonation.ViewModels;

namespace BloodDonation.Repository;

public interface IDonationRepository
{
    Task<Donation?> Find(long id);
    Task<IEnumerable<Donation>> GetRequests();
    Task<IEnumerable<Donation>> GetRecentDonations(long donorId, int count = 5);
    Task<PagedResult<Donation>> GetDonations(DonationSearchVm vm, long? donorId);
}