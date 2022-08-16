using BloodDonation.Constants;
using BloodDonation.Dto;
using BloodDonation.Repository.Interfaces;
using BloodDonation.Seeder.Interfaces;
using BloodDonation.Services.Interfaces;

namespace BloodDonation.Seeder;

public class UserSeeder : IUserSeeder
{
    private readonly IUserService _userService;
    private readonly IUserRepository _userRepo;

    public UserSeeder(IUserService userService, IUserRepository userRepo)
    {
        _userService = userService;
        _userRepo = userRepo;
    }

    public async Task SeedAdminUser()
    {
        var adminUsers = await _userRepo.GetUsersByType(UserLevels.Admin);
        if (!adminUsers.Any())
        {
            var dto = new UserDto()
            {
                UserName = "admin",
                Password = "Admin@123",
                UserLevel = UserLevels.Admin
            };
            await _userService.Create(dto);
        }
    }
}