using BloodDonation.Constants;

namespace BloodDonation.Dto;

public class DonationCreateDto
{
    public long? UserDetailsId { get; set; }

    public DateTime Date { get; set; }
    public string Name { get; set; }
    public string BloodGroup { get; set; }
    public string ContactNo { get; set; }

    public string DonationDistrict { get; set; }
    public string DonationLocation { get; set; }
    public string Receiver { get; set; }

    public string Type { get; set; } = DonationTypes.Donation;

    public string Status { get; set; } = DonationStatus.Unverified;
    
    public IFormFile? File { get; set; }
}