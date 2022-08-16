using System.Security.Claims;
using BloodDonation.Crypter.Interfaces;
using BloodDonation.Manager.Interfaces;
using BloodDonation.Repository.Interfaces;
using BloodDonation.Results;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace BloodDonation.Manager;

public class AuthManager : IAuthManager
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _userRepository;
    private readonly ICrypter _crypter;

    public AuthManager(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, ICrypter crypter)
    {
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
        _crypter = crypter;
    }

    public async Task<AuthResult> Login(string identity, string password)
    {
        var user = await _userRepository.GetByUserName(identity);
        var result = new AuthResult();
        if (user == null)
        {
            result.Success = false;
            result.Errors.Add("User not found");
            return result;
        }

        if (!_crypter.Verify(password, user.Password))
        {
            result.Success = false;
            result.Errors.Add("Invalid password");
            return result;
        }

        var httpContext = _httpContextAccessor.HttpContext;
        var claims = new List<Claim>
        {
            new("Id", user.Id.ToString())
        };
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await httpContext!.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity));
        result.Success = true;
        return result;
    }

    public async Task Logout()
    {
        await _httpContextAccessor.HttpContext!.SignOutAsync();
    }
}