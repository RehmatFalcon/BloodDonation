using BloodDonation.Dto;
using BloodDonation.Models;

namespace BloodDonation.Services.Interfaces;

public interface IDonationService
{
    Task<Donation> Create(DonationCreateDto dto);
    Task Approve(Donation donation);
    Task Delete(Donation donation);
}