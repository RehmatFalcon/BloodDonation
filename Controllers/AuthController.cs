using AspNetCoreHero.ToastNotification.Abstractions;
using BloodDonation.Manager.Interfaces;
using BloodDonation.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloodDonation.Controllers;

public class AuthController : Controller
{
    private readonly IAuthManager _authManager;
    private readonly INotyfService _notyfService;

    public AuthController(IAuthManager authManager, INotyfService notyfService)
    {
        _authManager = authManager;
        _notyfService = notyfService;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Login()
    {
        var vm = new LoginVm();
        return View(vm);
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login(LoginVm vm)
    {
        try
        {
            var result = await _authManager.Login(vm.Username, vm.Password);
            if (result.Success)
            {
                _notyfService.Success($"Welcome, {vm.Username}");
                return Redirect("/");
            }

            ModelState.AddModelError(nameof(vm.Password), string.Join(",", result.Errors));
            return View(vm);
        }
        catch (Exception e)
        {
            _notyfService.Error(e.Message);
            return RedirectToAction("Login");
        }
    }


    public async Task<IActionResult> Logout()
    {
        await _authManager.Logout();
        _notyfService.Information("Session logged out");
        return Redirect("/");
    }
}