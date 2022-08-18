namespace BloodDonation.Dto;

public class DonorCreateDto : UserDto
{
    #region Required

    public string Name { get; set; }
    public string District { get; set; }
    public string ContactNo { get; set; }
    public string BloodGroup { get; set; }

    #endregion

    public string Address { get; set; }
    public string Note { get; set; }
    public DateTime? LastDonationDate { get; set; }

    public long DonationCount { get; set; } = 0;
}