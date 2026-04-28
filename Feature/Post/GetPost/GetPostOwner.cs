using System;
using blogger_clone.Dto;
using blogger_clone.Extension;
using blogger_clone.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace blogger_clone.Feature.Post.GetPost;


public record QueryOwner() : IRequest<ResultOwner>;
public record ResultOwner(
    IEnumerable<PostDtoOwner> Data
);

public class GetPostOwner(
    AppDbContext dbContext,
    IUtils util,
    IHttpContextAccessor httpContextAccessor
) : IRequestHandler<QueryOwner, ResultOwner>

{
    public static void MapEndPoint(RouteGroupBuilder group)
    {
        group.MapGet("/all-post", async(
            ISender sender
        ) =>
        {
            return Results.Ok(await sender.Send(new QueryOwner()));
        })
        .WithName("Get all Post (Owner)")
        .Produces<ResultOwner>(StatusCodes.Status200OK)
        .RequireAuthorization();
    }

    public async Task<ResultOwner> Handle (QueryOwner req, CancellationToken ct)
    {
        var blogId = httpContextAccessor.HttpContext!.Items["BlogId"] as Guid?;

        var userId = util.GetUserId();

        var post = await dbContext.Post
        .Include(t => t.Blog)
        .Where(t => t.BlogId == blogId
        && t.Blog!.UserId == userId)
        .Select(t => new PostDtoOwner(
            Id: t.Id,
            Title: t.Title,
            Status: t.Status,
            CreatedAt: t.CreatedAt ?? DateTime.MinValue,
            Content: t.Content
        ))
        .ToListAsync(ct);

        var response = new ResultOwner (
            Data: post
        );

        return response;
    }
}
