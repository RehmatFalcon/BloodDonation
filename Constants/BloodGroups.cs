using Microsoft.AspNetCore.Mvc.Rendering;

namespace BloodDonation.Constants;

public static class BloodGroups
{
    public static List<string> Value = new()
    {
        "A+",
        "A-",
        "B+",
        "B-",
        "O+",
        "O-",
        "AB+",
        "AB-"
    };

    public static SelectList GetSelectList(string selectedValue) => new SelectList(Value, selectedValue);
}