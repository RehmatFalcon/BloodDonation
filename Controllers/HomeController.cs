using System.Diagnostics;
using BloodDonation.Provider.Interfaces;
using BloodDonation.Repository.Interfaces;
using BloodDonation.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BloodDonation.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IDonorRepository _donorRepo;

    public HomeController(ILogger<HomeController> logger, ICurrentUserProvider currentUserProvider,
        IDonorRepository donorRepo)
    {
        _logger = logger;
        _currentUserProvider = currentUserProvider;
        _donorRepo = donorRepo;
    }

    public async Task<IActionResult> Index()
    {
        var vm = new DashboardVm();
        vm.IsAdmin = !await _currentUserProvider.IsDonor();
        if (!vm.IsAdmin)
        {
            vm.Donor = await _currentUserProvider.GetCurrentDonor();
        }

        return View(vm);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}