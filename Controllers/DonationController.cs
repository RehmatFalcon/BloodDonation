using AspNetCoreHero.ToastNotification.Abstractions;
using BloodDonation.Constants;
using BloodDonation.Dto;
using BloodDonation.Provider.Interfaces;
using BloodDonation.Repository;
using BloodDonation.Services.Interfaces;
using BloodDonation.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BloodDonation.Controllers;

public class DonationController : Controller
{
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IDonationService _donationService;
    private readonly IDonationRepository _donationRepo;
    private readonly INotyfService _notyfService;

    public DonationController(ICurrentUserProvider currentUserProvider, IDonationService donationService,
        IDonationRepository donationRepo, INotyfService notyfService)
    {
        _currentUserProvider = currentUserProvider;
        _donationService = donationService;
        _donationRepo = donationRepo;
        _notyfService = notyfService;
    }

    public async Task<IActionResult> Index(DonationSearchVm vm)
    {
        try
        {
            var isDonor = await _currentUserProvider.IsDonor();
            long? donorId = null;
            if (vm.IsSelf && isDonor)
            {
                donorId = (await _currentUserProvider.GetCurrentDonor()).Id;
            }

            vm.Result = await _donationRepo.GetDonations(vm, donorId);
            return View(vm);
        }
        catch (Exception e)
        {
            _notyfService.Error(e.Message);
            return Redirect("/");
        }
    }

    public async Task<IActionResult> New()
    {
        try
        {
            var vm = new DonationRecordVm();
            await AllowOnlyDonor();
            return View(vm);
        }
        catch (Exception e)
        {
            _notyfService.Error(e.Message);
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public async Task<IActionResult> New(DonationRecordVm vm)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            await AllowOnlyDonor();

            var dto = new DonationCreateDto()
            {
                Date = vm.Date.Date,
                Name = vm.Name,
                Receiver = vm.Receiver,
                ContactNo = vm.ContactNo,
                BloodGroup = vm.BloodGroup,
                DonationDistrict = vm.DonationDistrict,
                DonationLocation = vm.DonationLocation,
                UserDetailsId = (await _currentUserProvider.GetCurrentDonor())!.Id,
                Type = vm.Type,
                Status = DonationStatus.Unverified,
                File = vm.File
            };
            await _donationService.Create(dto);
            _notyfService.Success("Donation Recorded");
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _notyfService.Error(e.Message);
            return RedirectToAction("Index");
        }
    }

    public async Task AllowOnlyDonor()
    {
        if (!await _currentUserProvider.IsDonor())
        {
            throw new Exception("Only donors allowed");
        }
    }
}