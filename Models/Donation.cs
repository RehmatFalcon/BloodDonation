using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BloodDonation.Constants;

namespace BloodDonation.Models;

[Table("donation")]
public class Donation
{
    [Key] public long Id { get; set; }

    [ForeignKey("UserDetailsId")] public long? UserDetailsId { get; set; }

    public DateTime Date { get; set; }
    public string Name { get; set; }
    public string BloodGroup { get; set; }
    public string ContactNo { get; set; }

    public string DonationDistrict { get; set; }
    public string DonationLocation { get; set; }
    public string Receiver { get; set; }

    public string Type { get; set; } = DonationTypes.Donation;

    public string Status { get; set; } = DonationStatus.Unverified;

    public bool IsVerified() => Status == DonationStatus.Verified;

    public bool IsDonation() => Type == DonationTypes.Donation;
}