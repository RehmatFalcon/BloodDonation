using AspNetCoreHero.ToastNotification.Abstractions;
using BloodDonation.Provider.Interfaces;
using BloodDonation.Repository;
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
    private readonly IDonationRepository _donationRepository;
    private readonly IDonationService _donationService;

    public AdminController(IUserRepository userRepo, INotyfService notyfService, IUserService userService,
        ICurrentUserProvider currentUserProvider, IDonationRepository donationRepository,
        IDonationService donationService)
    {
        _userRepo = userRepo;
        _notyfService = notyfService;
        _userService = userService;
        _currentUserProvider = currentUserProvider;
        _donationRepository = donationRepository;
        _donationService = donationService;
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

    public async Task<IActionResult> DonationRequests()
    {
        var requests = await _donationRepository.GetRequests();
        return View(requests);
    }

    public async Task<IActionResult> ApproveRequest(long id)
    {
        try
        {
            var donation = await _donationRepository.Find(id);
            if (donation == null) throw new Exception("Donation not found");
            await _donationService.Approve(donation);
            _notyfService.Success($"Donation #{id} Approved");
            return RedirectToAction("DonationRequests");
        }
        catch (Exception e)
        {
            _notyfService.Error(e.Message);
            return Redirect("/");
        }
    }

    public async Task<IActionResult> RejectRequest(long id)
    {
        try
        {
            var donation = await _donationRepository.Find(id);
            if (donation == null) throw new Exception("Donation not found");
            await _donationService.Reject(donation);
            _notyfService.Information($"Donation #{id} Rejected");
            return RedirectToAction("DonationRequests");
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