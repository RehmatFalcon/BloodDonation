using Microsoft.AspNetCore.Mvc.Rendering;

namespace BloodDonation.Constants;

public static class DonationTypes
{
    public const string Donation = "Donation";
    public const string Receipt = "Receipt";

    public static readonly List<string> Value = new List<string>()
    {
        Donation,
        Receipt
    };

    public static SelectList GetSelectList(string selectedValue)
        => new SelectList(Value, selectedValue);
}