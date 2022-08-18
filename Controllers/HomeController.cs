using System.Diagnostics;
using BloodDonation.Provider.Interfaces;
using BloodDonation.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BloodDonation.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IDbConnectionProvider _connectionProvider;

    public HomeController(ILogger<HomeController> logger, IDbConnectionProvider connectionProvider)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }

    public async Task<IActionResult> Index()
    {
        return View();
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