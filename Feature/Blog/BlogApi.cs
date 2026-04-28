using System;
using blogger_clone.Feature.Blog.Admin.DeleteBlogAdmin;
using blogger_clone.Feature.Blog.Admin.GetBlogAdmin;
using blogger_clone.Feature.Blog.UserGetBlog;
using Carter;

namespace blogger_clone.Feature.Blog;

public class BlogApi : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/blog")
        .WithTags("Blog Management");

        var groupAdmin = app.MapGroup("api/v1/admin/blog")
        .RequireAuthorization(p => p.RequireRole("Admin"))
        .WithTags("Blog Management (Admin)");

        CreateBlog.CreateBlog.MapEndpoint(group);
        DeleteBlog.DeleteBlog.MapEndpoint(group);
        GetAllBlog.MapEndpoint(group);

        GetBlogAdmin.MapEndpoint(groupAdmin);
        DeleteBlogAdmin.MapEndpoint(groupAdmin);
    }
}
