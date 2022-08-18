namespace BloodDonation.ViewModels;

public class UserChangePasswordVm
{
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
    public string RepeatPassword { get; set; }
}