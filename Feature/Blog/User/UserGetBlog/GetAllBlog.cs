using System;
using blogger_clone.Exception.Blog;
using blogger_clone.Extension;
using blogger_clone.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using blogger_clone.Dto;

namespace blogger_clone.Feature.Blog.UserGetBlog;

public class GetAllBlog(
    AppDbContext _context,
    IUtils _utils,
    IConfiguration _config
) : IRequestHandler<Query, Result>

{
    public async Task<Result> Handle (Query req, CancellationToken ct)
    {
        var userId = _utils.GetUserId();
        
        var blog = await _context.Blog.Where(t => t.UserId == userId)
        .Select(t => new BlogDto(
            BlogId: t.Id,
            SubDomain: t.SubDomain,
            Domain: $"{t.SubDomain}.{_config["AppConfig:BaseDomain"]}"
        ))
        .ToListAsync(ct);

        var response = new Result(blog);

        return response;
    }

    public static void MapEndpoint (RouteGroupBuilder group)
    {
        group.MapGet("/list-all", async(
            ISender sender
        ) =>
        {
            var result = await sender.Send(new Query());

            return Results.Ok(result);
        })
        .WithName("Get all Blog (User)")
        .RequireAuthorization()
        .Produces<Result>(StatusCodes.Status200OK);
    }
}
