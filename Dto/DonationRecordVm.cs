using System.ComponentModel;
using BloodDonation.Constants;

namespace BloodDonation.Dto;

public class DonationRecordVm
{
    [DisplayName("Date")] public DateTime Date { get; set; } = DateTime.UtcNow;

    [DisplayName("Donor/Recipient")] public string Name { get; set; }

    [DisplayName("Blood Group")] public string BloodGroup { get; set; }

    [DisplayName("Contact No")] public string ContactNo { get; set; }


    [DisplayName("District")] public string DonationDistrict { get; set; }

    [DisplayName("Location (Organization/Program)")]
    public string DonationLocation { get; set; }


    [DisplayName("Secondary Party")] public string Receiver { get; set; }


    [DisplayName("Donation/Receipt")] public string Type { get; set; } = DonationTypes.Donation;
}