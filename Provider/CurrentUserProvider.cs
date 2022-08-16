using System.Security.Claims;
using BloodDonation.Models;
using BloodDonation.Provider.Interfaces;
using BloodDonation.Repository.Interfaces;

namespace BloodDonation.Provider;

public class CurrentUserProvider : ICurrentUserProvider
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IUserRepository _userRepository;

    public CurrentUserProvider(IHttpContextAccessor contextAccessor, IUserRepository userRepository)
    {
        _contextAccessor = contextAccessor;
        _userRepository = userRepository;
    }

    public bool IsLoggedIn()
        => GetCurrentUserId() != null;

    public async Task<User?> GetCurrentUser()
    {
        var userId = GetCurrentUserId();
        if (userId.HasValue) return await _userRepository.Find(userId.Value);
        return null;
    }

    public long? GetCurrentUserId()
    {
        var userId = _contextAccessor.HttpContext?.User.FindFirstValue("Id");
        if (string.IsNullOrWhiteSpace(userId)) return null;
        return Convert.ToInt64(userId);
    }
}