namespace BloodDonation.Crypter.Interfaces;

public interface ICrypter
{
    string Hash(string password);
    bool Verify(string password, string hash);
}