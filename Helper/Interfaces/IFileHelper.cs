namespace BloodDonation.Helper.Interfaces;

public interface IFileHelper
{
    Task<string> Save(IFormFile file);
}