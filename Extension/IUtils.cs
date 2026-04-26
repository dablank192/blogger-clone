using System;
using blogger_clone.Model;


namespace blogger_clone.Extension;

public interface IUtils
{
    public string GenerateRandomUsername();
    public string PasswordHasher (string password);
    public bool VerifyPassword (string plainPassword, string hashedPassword);
    public string GenerateJwtToken(User user);
    public string ToSubdomain(string username);
    public Guid GetUserId ();
}
