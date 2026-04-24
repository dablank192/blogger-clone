using System;
using Microsoft.AspNetCore.Identity;


namespace blogger_clone.Extension;

public class Utils : IUtils
{
    public string GenerateRandomUsername()
    {
        var randNum = Random.Shared.Next(0, 1000000);

        var randId = randNum.ToString("D8");

        var userId = $"user_{randId}";

        return userId;
    }

    public string PasswordHasher(string password)
    {
        PasswordHasher<object> hasher = new();

        var hashedPassword = hasher.HashPassword(new object(), password);

        return hashedPassword;
    }

    public bool VerifyPassword(string plainPassword, string hashedPassword)
    {
        PasswordHasher<object> hasher = new();

        var verifyPassword = hasher.VerifyHashedPassword(new object(), hashedPassword, plainPassword);

        if (verifyPassword == PasswordVerificationResult.Failed)
        {
            return false;
        }

        return true;
    }
}
