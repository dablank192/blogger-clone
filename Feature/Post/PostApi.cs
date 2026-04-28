using System;
using blogger_clone.Feature.Post.GetPost;
using Carter;

namespace blogger_clone.Feature.Post;

public class PostApi : ICarterModule
{
    public void AddRoutes (IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/post")
        .WithTags("Post Management");

        var groupManage = app.MapGroup("api/v1/owner/post")
        .WithTags("Post Management (Owner)");

        CreatePost.CreatePost.MapEndPoint(group);
        GetPost.GetPost.MapEndPoint(group);
        DeletePost.DeletePost.MapEndPoint(group);

        GetPostOwner.MapEndPoint(groupManage);
    }
}
