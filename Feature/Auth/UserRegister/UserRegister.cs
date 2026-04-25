using System;
using blogger_clone.Exception.Auth;
using blogger_clone.Extension;
using blogger_clone.Infrastructure;
using blogger_clone.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace blogger_clone.Feature.Auth.UserRegister;

public class UserRegister : IRequestHandler<Command, Result>
{
    private readonly AppDbContext _context;
    private readonly IUtils _utils;

    public UserRegister(AppDbContext context, IUtils utils)
    {
        _context = context;
        _utils = utils;
    }

    public async Task<Result> Handle (Command req, CancellationToken ct)
    {
        var userEmail = await _context.User.FirstOrDefaultAsync(t => t.Email == req.Email, ct);
        
        if (userEmail != null)
        {
            throw new EmailExistedException();
        }

        var hashedPassword = _utils.PasswordHasher(req.Password);

        var newUser = new User
        {
            Email= req.Email,
            Username= _utils.GenerateRandomUsername(),
            Password= hashedPassword
        };

        _context.User.Add(newUser);

        await _context.SaveChangesAsync(ct);

        var response = new Result (
            Id: newUser.Id,
            Message: "User created successfully"
        );

        return response;
    }

    public static void MapEndpoint (RouteGroupBuilder group)
    {
        group.MapPost("/register", async (
            Command req,
            ISender sender
        ) => {
            var result = await sender.Send(req);

            return Results.Ok(result);
        })
        .WithName("User Register")
        .Produces<Result>(StatusCodes.Status200OK)
        .ProducesValidationProblem();
    }
}
