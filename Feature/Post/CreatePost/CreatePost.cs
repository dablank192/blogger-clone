using System;
using blogger_clone.Exception.Blog;
using blogger_clone.Extension;
using blogger_clone.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace blogger_clone.Feature.Post.CreatePost;

public class CreatePost(
    AppDbContext dbContext,
    IUtils utils
) : IRequestHandler<Command, Result>

{
    public static void MapEndPoint(RouteGroupBuilder group)
    {
        group.MapPost("/new-post", async(
            Command req,
            ISender sender
        ) =>
        {
            var result = await sender.Send(req);

            return Results.Ok(result);
        })
        .WithName("Create Post")
        .RequireAuthorization()
        .Produces<Result>(StatusCodes.Status200OK);
    }

    public async Task<Result> Handle (Command req, CancellationToken ct)
    {
        var userId = utils.GetUserId();

        var blogId = await dbContext.Blog.Where(t => t.UserId == userId)
        .Select(t  => t.Id)
        .FirstOrDefaultAsync(ct);

        if(blogId == Guid.Empty) throw new BlogNotExistedException();

        var newPost = new Model.Post
        {
            BlogId = blogId,
            Title= req.Title,
            Content= req.Content
        };

        dbContext.Post.Add(newPost);
        await dbContext.SaveChangesAsync(ct);

        var response = new Result (
            PostId: newPost.Id,
            Message: "Post created successfully"
        );

        return response;
    }
}
