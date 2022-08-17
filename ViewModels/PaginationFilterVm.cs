namespace BloodDonation.ViewModels;

public class PaginationFilterVm
{
    public int Page { get; set; } = 1;
    public int Limit { get; set; } = 100;
}