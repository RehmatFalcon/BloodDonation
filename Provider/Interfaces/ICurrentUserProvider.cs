using BloodDonation.Models;

namespace BloodDonation.Provider.Interfaces;

public interface ICurrentUserProvider
{
    bool IsLoggedIn();
    Task<User?> GetCurrentUser();
    long? GetCurrentUserId();

    Task<UserDetails?> GetCurrentDonor();

    Task<bool> IsDonor();
}