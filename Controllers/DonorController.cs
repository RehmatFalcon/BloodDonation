using BloodDonation.Repository.Interfaces;
using BloodDonation.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BloodDonation.Controllers;

public class DonorController : Controller
{
    private readonly IDonorRepository _donorRepo;

    public DonorController(IDonorRepository donorRepo)
    {
        _donorRepo = donorRepo;
    }

    public async Task<IActionResult> Index(DonorSearchVm vm)
    {
        vm.Result = await _donorRepo.SearchDonors(vm);
        return View(vm);
    }
}