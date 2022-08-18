using AspNetCoreHero.ToastNotification.Abstractions;
using BloodDonation.Provider.Interfaces;
using BloodDonation.Services.Interfaces;
using BloodDonation.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BloodDonation.Controllers;

public class ProfileController : Controller
{
    private readonly IUserService _userService;
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly INotyfService _notyfService;

    public ProfileController(IUserService userService, ICurrentUserProvider currentUserProvider,
        INotyfService notyfService)
    {
        _userService = userService;
        _currentUserProvider = currentUserProvider;
        _notyfService = notyfService;
    }

    public IActionResult ChangePassword()
    {
        var vm = new UserChangePasswordVm();
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(UserChangePasswordVm vm)
    {
        try
        {
            var user = await _currentUserProvider.GetCurrentUser();
            if (vm.NewPassword != vm.RepeatPassword)
            {
                ModelState.AddModelError(nameof(UserChangePasswordVm.RepeatPassword),
                    "New password and repeat password do not match");
                return View(vm);
            }

            await _userService.ChangePassword(user!, vm.OldPassword, vm.NewPassword);
            _notyfService.Success("Password changed");
            return Redirect("/");
        }
        catch (Exception e)
        {
            _notyfService.Error(e.Message);
            return Redirect("/");
        }
    }
}