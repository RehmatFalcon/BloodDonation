using AspNetCoreHero.ToastNotification.Abstractions;
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
}