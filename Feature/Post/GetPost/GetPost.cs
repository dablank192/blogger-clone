using System;
using blogger_clone.Dto;
using blogger_clone.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace blogger_clone.Feature.Post.GetPost;


public record Query() : IRequest<Result>;
public record Result(
    IEnumerable<PostDto> Data
);

public class GetPost(
    AppDbContext dbContext,
    IHttpContextAccessor httpContextAccessor
) : IRequestHandler<Query, Result>

{
    public static void MapEndPoint(RouteGroupBuilder group)
    {
        group.MapGet("/all-post", async(
            ISender sender
        ) =>
        {
            return Results.Ok(await sender.Send(new Query()));
        })
        .WithName("Get all Post")
        .Produces<Result>(StatusCodes.Status200OK)
        .AllowAnonymous();
    }

    public async Task<Result> Handle (Query req, CancellationToken ct)
    {
        var blogId = httpContextAccessor.HttpContext!.Items["BlogId"] as Guid?;

        var post = await dbContext.Post
        .Where(t => t.BlogId == blogId)
        .Select(t => new PostDto(
            Id: t.Id,
            Title: t.Title,
            Content: t.Content
        ))
        .ToListAsync(ct);

        var response = new Result (
            Data: post
        );

        return response;
    }
}
