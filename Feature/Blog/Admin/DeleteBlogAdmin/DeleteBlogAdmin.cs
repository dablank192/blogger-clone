using System;
using blogger_clone.Exception.Blog;
using blogger_clone.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace blogger_clone.Feature.Blog.Admin.DeleteBlogAdmin;


public record Query(Guid BlogId) : IRequest;

public class DeleteBlogAdmin(
    AppDbContext _context
) : IRequestHandler<Query>

{
    public async Task Handle (Query req, CancellationToken ct)
    {
        var blog = await _context.Blog.FirstOrDefaultAsync(t => t.Id == req.BlogId, ct)
        ?? throw new BlogNotExistedException();

        _context.Blog.Remove(blog);

        await _context.SaveChangesAsync(ct);
    }

    public static void MapEndpoint (RouteGroupBuilder group)
    {
        group.MapDelete("/{blogId}", async(
            Guid blogId,
            ISender sender
        ) =>
        {
            await sender.Send(new Query(blogId));

            return Results.NoContent();
        })
        .RequireAuthorization(policy => policy.RequireRole("Admin"))
        .WithName("Delete Blog (Admin)")
        .Produces(StatusCodes.Status204NoContent);
    }
}
