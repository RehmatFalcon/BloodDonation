using AspNetCoreHero.ToastNotification.Abstractions;
using BloodDonation.Dto;
using BloodDonation.Provider.Interfaces;
using BloodDonation.Services.Interfaces;
using BloodDonation.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BloodDonation.Controllers;

public class ProfileController : Controller
{
    private readonly IUserService _userService;
    private readonly IDonorService _donorService;
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly INotyfService _notyfService;

    public ProfileController(IUserService userService, IDonorService donorService,
        ICurrentUserProvider currentUserProvider,
        INotyfService notyfService)
    {
        _userService = userService;
        _donorService = donorService;
        _currentUserProvider = currentUserProvider;
        _notyfService = notyfService;
    }

    public async Task<IActionResult> UpdateProfile()
    {
        try
        {
            var donor = await _currentUserProvider.GetCurrentDonor();
            var vm = new UpdateProfileVm()
            {
                Donor = donor,
                Name = donor.Name,
                Address = donor.Address,
                District = donor.District,
                Note = donor.Note,
                BloodGroup = donor.BloodGroup,
                ContactNo = donor.ContactNo
            };
            return View(vm);
        }
        catch (Exception e)
        {
            _notyfService.Error(e.Message);
            return Redirect("/");
        }
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProfile(UpdateProfileVm vm)
    {
        try
        {
            var donor = await _currentUserProvider.GetCurrentDonor();
            if (!ModelState.IsValid)
            {
                vm.Donor = donor;
                return View(vm);
            }

            var dto = new DonorUpdateDto()
            {
                Name = vm.Name,
                Address = vm.Address,
                District = vm.District,
                Note = vm.Note,
                BloodGroup = vm.BloodGroup,
                ContactNo = vm.ContactNo,
            };
            await _donorService.Update(donor.Id, dto);
            _notyfService.Success("Profile Updated");
            return RedirectToAction("UpdateProfile");
        }
        catch (Exception e)
        {
            _notyfService.Error(e.Message);
            return Redirect("/");
        }
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