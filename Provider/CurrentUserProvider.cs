using System.Security.Claims;
using BloodDonation.Constants;
using BloodDonation.Models;
using BloodDonation.Provider.Interfaces;
using BloodDonation.Repository.Interfaces;

namespace BloodDonation.Provider;

public class CurrentUserProvider : ICurrentUserProvider
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IUserRepository _userRepository;
    private readonly IDonorRepository _donorRepository;

    private User? _currentUser;
    private UserDetails? _currentDonor;

    public CurrentUserProvider(IHttpContextAccessor contextAccessor, IUserRepository userRepository,
        IDonorRepository donorRepository)
    {
        _contextAccessor = contextAccessor;
        _userRepository = userRepository;
        _donorRepository = donorRepository;
    }

    public bool IsLoggedIn()
        => GetCurrentUserId() != null;

    public async Task<User?> GetCurrentUser()
    {
        if (_currentUser == null)
        {
            var userId = GetCurrentUserId();
            if (userId.HasValue) _currentUser = await _userRepository.Find(userId.Value);
        }

        return _currentUser;
    }

    public long? GetCurrentUserId()
    {
        var userId = _contextAccessor.HttpContext?.User.FindFirstValue("Id");
        if (string.IsNullOrWhiteSpace(userId)) return null;
        return Convert.ToInt64(userId);
    }

    public async Task<bool> IsDonor() => (await GetCurrentUser())?.UserLevel == UserLevels.NormalUser;

    public async Task<UserDetails?> GetCurrentDonor()
    {
        if (_currentDonor == null)
        {
            var userId = GetCurrentUserId();
            if (userId.HasValue)
            {
                _currentDonor = await _donorRepository.GetByUserId(userId.Value);
            }
        }

        return _currentDonor;
    }
}