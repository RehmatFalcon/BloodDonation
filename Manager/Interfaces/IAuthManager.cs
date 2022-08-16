using BloodDonation.Results;

namespace BloodDonation.Manager.Interfaces;

public interface IAuthManager
{
    Task<AuthResult> Login(string identity, string password);
    Task Logout();
}