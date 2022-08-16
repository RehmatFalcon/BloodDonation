using BloodDonation.Seeder.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloodDonation.Controllers;

[AllowAnonymous]
public class SeedController : Controller
{
    private readonly IUserSeeder _userSeeder;

    public SeedController(IUserSeeder userSeeder)
    {
        _userSeeder = userSeeder;
    }

    public async Task<IActionResult> SeedAdminUser()
    {
        try
        {
            await _userSeeder.SeedAdminUser();
            return Content("User Seeded");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}