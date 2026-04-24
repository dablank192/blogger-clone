using System;
using blogger_clone.Exception.Auth;
using blogger_clone.Extension;
using blogger_clone.Infrastructure;
using blogger_clone.Model;
using Microsoft.EntityFrameworkCore;

namespace blogger_clone.Feature.Auth.UserRegister;

public static class UserRegister
{
    public static void MapEndpoint (RouteGroupBuilder group)
    {
        
    }

    public static async Task<IResult> HandleAsync(
        RequestModel req,
        CancellationToken ct,
        AppDbContext _context,
        IUtils _utils
    )
    {
        var userEmail = await _context.User.FirstOrDefaultAsync(t => t.Email == req.Email, ct)
        ?? throw new EmailExistedException();

        var hashedPassword = _utils.PasswordHasher(req.Password);

        _context.User.Add(new User
        {
            Email= req.Email,
            Username= _utils.GenerateRandomUsername(),
            Password= hashedPassword
        });

        await _context.SaveChangesAsync(ct);

        var response = new ResponseModel(
            UserId: userEmail.Id,
            Message: "User created successfully"
        );

        return Results.Ok(response);
    }
}
