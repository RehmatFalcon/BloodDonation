using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BloodDonation.Models;

[Table("user_details")]
public class UserDetails
{
    #region Required

    [Key] public long Id { get; set; }

    [ForeignKey("UserId")] public long UserId { get; set; }

    public string Name { get; set; }
    public string District { get; set; }
    public string ContactNo { get; set; }
    public string BloodGroup { get; set; }

    #endregion

    public string Address { get; set; }
    public string Note { get; set; }
    public DateTime? LastDonationDate { get; set; }
}