using BloodDonation.Models;
using BloodDonation.Results;

namespace BloodDonation.ViewModels;

public class DonorSearchVm : PaginationFilterVm
{
    public PagedResult<UserDetails> Result;
    public string Name { get; set; }
    public string BloodGroup { get; set; } = "";

    public string ContactNo { get; set; }
    public string District { get; set; } = "";
    public string Address { get; set; }

    // Show only those whose last donation date is before three months
    public bool ShowEligibleOnly { get; set; } = true;
}