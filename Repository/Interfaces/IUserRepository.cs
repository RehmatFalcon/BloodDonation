using BloodDonation.Models;

namespace BloodDonation.Repository.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetUsersByType(string type);
}