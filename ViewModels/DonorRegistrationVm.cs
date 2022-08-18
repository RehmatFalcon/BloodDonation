using BloodDonation.Constants;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BloodDonation.ViewModels;

public class DonorRegistrationVm
{
    public string UserName { get; set; }
    public string Password { get; set; }

    #region Required

    public string Name { get; set; }
    public string District { get; set; }
    public string ContactNo { get; set; }
    public string BloodGroup { get; set; }

    #endregion

    public string Address { get; set; }
    public string Note { get; set; }
    public DateTime? LastDonationDate { get; set; }
    public long DonationCount { get; set; }

    public SelectList BloodGroupSelectList() => new SelectList(BloodGroups.Value, BloodGroup);
    public SelectList DistrictSelectList() => new SelectList(Districts.Value, District);
}