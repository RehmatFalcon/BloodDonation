using BloodDonation.Dto;
using BloodDonation.Models;

namespace BloodDonation.Services.Interfaces;

public interface IDonorService
{
    Task<UserDetails> Create(DonorCreateDto dto);
    Task Update(long userDetailsId, DonorUpdateDto dto);
}