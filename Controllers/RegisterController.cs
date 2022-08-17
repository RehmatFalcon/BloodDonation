using AspNetCoreHero.ToastNotification.Abstractions;
using BloodDonation.Dto;
using BloodDonation.Services.Interfaces;
using BloodDonation.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloodDonation.Controllers;

[AllowAnonymous]
public class RegisterController : Controller
{
    private readonly IDonorService _donorService;
    private readonly INotyfService _notyfService;

    public RegisterController(IDonorService donorService, INotyfService notyfService)
    {
        _donorService = donorService;
        _notyfService = notyfService;
    }

    public IActionResult Index()
    {
        var vm = new DonorRegistrationVm();
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Index(DonorRegistrationVm vm)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var dto = new DonorCreateDto()
            {
                UserName = vm.UserName,
                Password = vm.Password,
                Name = vm.Name,
                Address = vm.Address,
                District = vm.District,
                Note = vm.Note,
                BloodGroup = vm.BloodGroup,
                ContactNo = vm.ContactNo,
                LastDonationDate = vm.LastDonationDate
            };
            await _donorService.Create(dto);
            _notyfService.Success($"Congrats, {vm.UserName}. Registration complete. You can login now.");
            return Redirect("/");
        }
        catch (Exception e)
        {
            _notyfService.Error(e.Message);
            return View(vm);
        }
    }
}