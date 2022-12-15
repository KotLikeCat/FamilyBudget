using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace FamilyBudget.Common.Tools;

public static class PasswordTools
{
    private const string PasswordSalt = "gXgCBP6kfe+sduMQuI8bRw==";
    
    public static string HashPassword(string password)
    {
        var salt = Convert.FromBase64String(PasswordSalt);
        var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password!,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));

        return hashed;
    }
}