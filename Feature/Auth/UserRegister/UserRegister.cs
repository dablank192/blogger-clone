using System;
using blogger_clone.Exception.Auth;
using blogger_clone.Extension;
using blogger_clone.Infrastructure;
using blogger_clone.Model;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;


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
        group.MapGet("/register", async()
        =>
        {
            return new RazorComponentResult<RegisterPageView>();
        });


        group.MapPost("/register", async Task<IResult>(
            [FromForm]Command req,
            [FromServices]ISender sender
        ) => {
                try
                {
                    await sender.Send(req);

                    return new RazorComponentResult<RegisterSuccessView>();
                }
                catch(ValidationException ex)
                {
                    var error = ex.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.First().ErrorMessage);

                    return new RazorComponentResult<RegisterPageView>(new
                    {
                        errors= error,
                        submittedEmail= req.Email
                    });
                }
            })
        .WithName("User Register")
        .Produces<Result>(StatusCodes.Status200OK)
        .ProducesValidationProblem();
    }
}
