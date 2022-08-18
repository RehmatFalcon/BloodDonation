using Microsoft.AspNetCore.Mvc.Rendering;

namespace BloodDonation.Helper;

public static class SelectListHelper
{
    public static IEnumerable<SelectListItem> AddDefaultOption(this SelectList selectList) =>
        selectList.Prepend(new SelectListItem("Select", ""));
}