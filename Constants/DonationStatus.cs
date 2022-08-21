using Microsoft.AspNetCore.Mvc.Rendering;

namespace BloodDonation.Constants;

public static class DonationStatus
{
    public const string Unverified = "Unverified";
    public const string Verified = "Verified";

    public static readonly List<string> Value = new List<string>()
    {
        Unverified,
        Verified
    };

    public static SelectList GetSelectList(string selectedValue)
        => new SelectList(Value, selectedValue);
}