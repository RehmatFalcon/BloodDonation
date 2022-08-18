using AspNetCoreHero.ToastNotification.Abstractions;
using BloodDonation.Provider.Interfaces;
using BloodDonation.Repository.Interfaces;
using BloodDonation.Services.Interfaces;
using BloodDonation.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BloodDonation.Controllers;

public class AdminController : Controller
{
    private readonly IUserRepository _userRepo;
    private readonly INotyfService _notyfService;
    private readonly IUserService _userService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public AdminController(IUserRepository userRepo, INotyfService notyfService, IUserService userService,
        ICurrentUserProvider currentUserProvider)
    {
        _userRepo = userRepo;
        _notyfService = notyfService;
        _userService = userService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<IActionResult> ResetPassword(long id)
    {
        try
        {
            await ValidateAccess();
            var user = await _userRepo.Find(id);
            if (user == null) throw new Exception("User Not Found");
            var newPassword = Guid.NewGuid().ToString()[..8];
            await _userService.ResetPassword(id, newPassword);
            _notyfService.Success("Password Reset Complete");
            var vm = new UserPasswordResetVm()
            {
                User = user,
                NewPassword = newPassword
            };
            return View(vm);
        }
        catch (Exception e)
        {
            _notyfService.Error(e.Message);
            return Redirect("/");
        }
    }

    private async Task ValidateAccess()
    {
        var isDonor = await _currentUserProvider.IsDonor();
        if (isDonor) throw new Exception("Cannot access admin section");
    }
}