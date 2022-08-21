using System.ComponentModel;
using BloodDonation.Constants;
using BloodDonation.Models;
using BloodDonation.Results;

namespace BloodDonation.ViewModels;

public class DonationSearchVm : PaginationFilterVm
{
    [DisplayName("Donation/Receipt")] public string? Type { get; set; }

    [DisplayName("Status")] public string? Status { get; set; } = DonationStatus.Verified;

    [DisplayName("From")] public DateTime From { get; set; } = DateTime.UtcNow.AddDays(-30);

    [DisplayName("To")] public DateTime To { get; set; } = DateTime.UtcNow;

    [DisplayName("Donor/Recipient")] public string? Name { get; set; }
    [DisplayName("Blood Group")] public string? BloodGroup { get; set; }
    [DisplayName("District")] public string? DonationDistrict { get; set; }

    [DisplayName("Location (Organization/Program)")]
    public string? DonationLocation { get; set; }

    [DisplayName("Secondary Party")] public string? Receiver { get; set; }

    // View Self
    [DisplayName("View Self Info")] public bool IsSelf { get; set; }

    public PagedResult<Donation> Result;
}