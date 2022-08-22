using BloodDonation.Helper.Interfaces;

namespace BloodDonation.Helper;

public class FileHelper : IFileHelper
{
    private readonly IWebHostEnvironment _hostingEnvironment;

    public FileHelper(IWebHostEnvironment hostingEnvironment)
    {
        _hostingEnvironment = hostingEnvironment;
    }

    public async Task<string> Save(IFormFile file)
    {
        var path = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads");
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        var fileName = Guid.NewGuid() + "." + Path.GetExtension(file.FileName);
        var fullPath = Path.Combine(path, fileName);
        await using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream);
        return fileName;
    }
}