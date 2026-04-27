using System;
using blogger_clone.Dto;
using blogger_clone.Extension;
using blogger_clone.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace blogger_clone.Feature.Blog.Admin.GetBlogAdmin;

public record Query() : IRequest<Result>;
public record Result(IEnumerable<BlogDtoAdmin> Data);

public class GetBlogAdmin(
    AppDbContext Context,
    IConfiguration Config
) : IRequestHandler<Query, Result>

{
    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapGet("/", async(
            ISender sender
        ) =>
        {
            return Results.Ok(await sender.Send(new Query()));
        })
        .WithName("Get all User's Blog (Admin)")
        .Produces<Result>(StatusCodes.Status200OK);
    }

    public async Task<Result> Handle (Query req, CancellationToken ct)
    {
        var blog = Context.Blog
        .Include(u => u.User);

        var listBlog = await blog.Select(t => new BlogDtoAdmin(
            UserId: t.UserId,
            BlogId: t.Id,
            SubDomain: t.SubDomain,
            Domain: $"{t.SubDomain}.{Config["AppConfig:BaseDomain"]}"
        ))
        .ToListAsync(ct);
        
        var response = new Result(
            Data: listBlog
        );

        return response;
    }
}
