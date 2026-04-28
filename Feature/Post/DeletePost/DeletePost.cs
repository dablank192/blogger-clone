using System;
using blogger_clone.Exception.Post;
using blogger_clone.Extension;
using blogger_clone.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace blogger_clone.Feature.Post.DeletePost;

public record Query(Guid PostId) : IRequest;


public class DeletePost(
    AppDbContext dbContext,
    IUtils utils
) : IRequestHandler<Query>

{
    public static void MapEndPoint(RouteGroupBuilder group)
    {
        group.MapDelete("/{PostId}", async(
            ISender sender,
            Guid PostId
        ) =>
        {
            await sender.Send(new Query(PostId));
            return Results.NoContent();
        })
        .WithName("Delete Post")
        .RequireAuthorization()
        .Produces(StatusCodes.Status204NoContent);
    }

    public async Task Handle(Query req, CancellationToken ct)
    {
        var userId = utils.GetUserId();

        var validPost = await dbContext.Post
        .Include(t => t.Blog)
        .Where(t => t.Blog!.UserId == userId
        && t.Id == req.PostId)
        .FirstOrDefaultAsync(ct)
        ?? throw new PostNotExistedException();

        dbContext.Post.Remove(validPost);
        await dbContext.SaveChangesAsync(ct);
    }
}
