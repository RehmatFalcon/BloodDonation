using BloodDonation.Crypter.Interfaces;

namespace BloodDonation.Crypter;

public class Crypter : ICrypter
{
    public string Hash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool Verify(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}