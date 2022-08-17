namespace BloodDonation.Dto;

public class DonorUpdateDto
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
}