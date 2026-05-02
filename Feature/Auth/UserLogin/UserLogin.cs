using System;
using System.Security.Authentication;
using blogger_clone.Extension;
using blogger_clone.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace blogger_clone.Feature.Auth.UserLogin;

public class UserLogin : IRequestHandler<Command, Result>
{
    private readonly AppDbContext _context;
    private readonly IUtils _utils;

    public UserLogin(AppDbContext context, IUtils utils)
    {
        _context = context;
        _utils = utils;
    }

    public async Task<Result> Handle(Command req, CancellationToken ct)
    {
        var user = await _context.User.FirstOrDefaultAsync(
            t => t.Username == req.UsernameOrEmail
            || t.Email == req.UsernameOrEmail,
            ct)
        ?? throw new InvalidCredentialException();

        var validPassword = _utils.VerifyPassword(req.Password, user.Password);

        if (validPassword == false)
        {
            throw new InvalidCredentialException();
        }

        var accessToken = _utils.GenerateJwtToken(user);

        var response = new Result(
            Id: user.Id,
            Token: accessToken
        );

        return response;
    }

    public static void MapEndpoint (RouteGroupBuilder group)
    {
        group.MapGet("/login", async()
        =>
        {
            return new RazorComponentResult<LoginPageView>();
        });


        group.MapPost("/login", async(
            Command req,
            ISender sender,
            HttpContext httpContext
        ) =>
        {
            var result = await sender.Send(req);

            httpContext.Response.Cookies.Append("AccessKey", result.Token, new CookieOptions
            {
                HttpOnly= true,
                Secure= false,
                SameSite= SameSiteMode.Strict,
                Expires= DateTime.UtcNow.AddHours(1)
            });

            httpContext.Response.Headers.Append("HX-Redirect", "/api/v1/home");

            return Results.Ok(result);
        })
        .WithName("User Login")
        .Produces<Result>(StatusCodes.Status200OK)
        .ProducesValidationProblem();
    }
}
