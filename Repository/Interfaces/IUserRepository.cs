using BloodDonation.Models;

namespace BloodDonation.Repository.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetUsersByType(string type);
    Task<User?> GetByUserName(string identity);
    Task<User?> Find(long userId);
}