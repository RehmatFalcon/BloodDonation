using BloodDonation.Models;

namespace BloodDonation.ViewModels;

public class DashboardVm
{
    public bool IsAdmin { get; set; }
    public UserDetails? Donor { get; set; }
}