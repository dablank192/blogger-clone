using System;
using blogger_clone.Exception.Blog;
using blogger_clone.Extension;
using blogger_clone.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace blogger_clone.Feature.Blog.DeleteBlog;

public class DeleteBlog(
    AppDbContext _context,
    IUtils _utils
) : IRequestHandler<Query>

{
    public async Task Handle (Query req, CancellationToken ct)
    {
        var userId = _utils.GetUserId();

        var blog = await _context.Blog
        .FirstOrDefaultAsync(t => t.UserId == userId, ct)
        ?? throw new BlogNotExistedException();

        _context.Blog.Remove(blog);

        await _context.SaveChangesAsync(ct);
    }

    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapDelete("/", async(
            ISender sender
        ) =>
        {
            await sender.Send(new Query());
            return Results.NoContent();
        })
        .WithName("Delete Blog")
        .RequireAuthorization()
        .Produces(StatusCodes.Status204NoContent);
    }
}
