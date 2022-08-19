using BloodDonation.Models;

namespace BloodDonation.ViewModels;

public class UpdateProfileVm
{
    public UserDetails? Donor { get; set; }
    
    #region Required

    public string Name { get; set; }
    public string District { get; set; }
    public string ContactNo { get; set; }
    public string BloodGroup { get; set; }

    #endregion

    public string Address { get; set; }
    public string Note { get; set; }
}