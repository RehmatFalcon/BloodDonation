using BloodDonation.Constants;
using BloodDonation.Models;
using BloodDonation.Results;
using BloodDonation.ViewModels;

namespace BloodDonation.Repository.Interfaces;

public interface IDonorRepository
{
    Task<PagedResult<UserDetails>> SearchDonors(DonorSearchVm vm);
    Task<UserDetails?> GetByUserId(long userId);
}