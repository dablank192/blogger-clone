using System;
using blogger_clone.Exception.Auth;
using blogger_clone.Extension;
using blogger_clone.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using blogger_clone.Exception.Blog;


namespace blogger_clone.Feature.Blog.CreateBlog;

public class CreateBlog(
    AppDbContext _context,
    IUtils _utils,
    IConfiguration _config)
    : IRequestHandler<Query, Result>
{
    public async Task<Result> Handle (Query req, CancellationToken ct)
    {
        var appConfig = _config.GetSection("AppConfig");

        var baseDomain = appConfig["BaseDomain"];

        var userId = _utils.GetUserId();
        
        var user = await _context.User
        .Include(t => t.Blog)
        .FirstOrDefaultAsync(t => t.Id == userId, ct)
        ?? throw new UserNotFoundException(userId);

        if (user.Blog.Any())
        {
            throw new BlogExistedException();
        }

        var subDomain = _utils.ToSubdomain(user.Username);

        var newBlog = new Model.Blog
        {
            UserId= userId,
            SubDomain= subDomain
        };

        _context.Blog.Add(newBlog);

        await _context.SaveChangesAsync(ct);

        var response = new Result (
            UserId: userId,
            SubDomain: $"http://{subDomain}.{baseDomain}"
        );

        return response;
    }

    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapGet("/create-domain", async(
            ClaimsPrincipal user,
            ISender sender
        ) =>
        {
            var result = await sender.Send(new Query());

            return Results.Ok(result);
        })
        .RequireAuthorization()
        .WithName("Create Blog")
        .Produces<Result>(StatusCodes.Status200OK);
    }
}
