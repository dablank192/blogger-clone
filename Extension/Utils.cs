using System;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using blogger_clone.Model;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;


namespace blogger_clone.Extension;

public class Utils : IUtils
{
    private readonly IConfiguration _config;
    private readonly IHttpContextAccessor _contextAccessor;

    public Utils (IConfiguration config, IHttpContextAccessor contextAccessor)
    {
        _config = config;
        _contextAccessor = contextAccessor;
    }
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

    public string GenerateJwtToken(User user)
    {
        var jwt = _config.GetSection("Jwt");

        var claims = new List<Claim>
        {
            new Claim("UserId", user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var jwtToken = new JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }

    public string ToSubdomain(string username)
    {
        if(string.IsNullOrWhiteSpace(username)) return string.Empty;

        var normalized = username.ToLowerInvariant().Normalize(NormalizationForm.FormD);

        var subdomain = new StringBuilder();

        foreach (var character in normalized)
        {
            if(System.Globalization.CharUnicodeInfo.GetUnicodeCategory(character) != System.Globalization.UnicodeCategory.NonSpacingMark)
            {
                subdomain.Append(character);
            }
        }
        normalized = subdomain.ToString().Normalize(NormalizationForm.FormC).Replace("đ", "d");

        return Regex.Replace(normalized, @"[^a-z0-9]", "");
    }

    public Guid GetUserId ()
    {
        var user = _contextAccessor.HttpContext!.User;

        var userIdString = user.FindFirstValue("UserId");

        if(string.IsNullOrEmpty(userIdString)) return Guid.Empty;

        var userId = Guid.Parse(userIdString!);

        return userId;
    }
}
