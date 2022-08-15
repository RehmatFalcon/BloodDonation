using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BloodDonation.Constants;

namespace BloodDonation.Models;

[Table("user")]
public class User
{
    [Key] public long Id { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public DateTime CreatedDate { get; set; }
    public string UserLevel { get; set; } = UserLevels.NormalUser;
}