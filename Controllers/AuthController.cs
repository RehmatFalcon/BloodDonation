using BloodDonation.Manager.Interfaces;
using BloodDonation.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloodDonation.Controllers;

public class AuthController : Controller
{
    private readonly IAuthManager _authManager;

    public AuthController(IAuthManager authManager)
    {
        _authManager = authManager;
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
                return Redirect("/");
            }

            ModelState.AddModelError(nameof(vm.Password), string.Join(",", result.Errors));
            return View(vm);
        }
        catch (Exception e)
        {
            return RedirectToAction("Login");
        }
    }


    public async Task<IActionResult> Logout()
    {
        await _authManager.Logout();
        return Redirect("/");
    }
}