using System;
using Carter;

namespace blogger_clone.Feature.Post;

public class PostApi : ICarterModule
{
    public void AddRoutes (IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/post")
        .WithTags("Post Management");

        CreatePost.CreatePost.MapEndPoint(group);
        GetPost.GetPost.MapEndPoint(group);
    }
}
