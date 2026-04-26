using System;
using blogger_clone.Feature.Blog.UserGetBlog;
using Carter;

namespace blogger_clone.Feature.Blog;

public class BlogApi : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/blog")
        .WithTags("Blog");

        CreateBlog.CreateBlog.MapEndpoint(group);
        DeleteBlog.DeleteBlog.MapEndpoint(group);
        GetAllBlog.MapEndpoint(group);
    }
}
