using BloodDonation.Dto;
using BloodDonation.Models;

namespace BloodDonation.Services.Interfaces;

public interface IUserService
{
    Task<User> Create(UserDto userDto);
}