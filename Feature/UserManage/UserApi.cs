using System;
using Carter;

namespace blogger_clone.Feature.UserManage;

public class UserApi : ICarterModule
{
    public void AddRoutes (IEndpointRouteBuilder app)
    {
        var groupUserManagement = app.MapGroup("api/v1/admin/user")
        .RequireAuthorization(p => p.RequireRole("Admin"))
        .WithTags("User Management (Admin)");

        GetAllUser.GetAllUser.MapEndPoint(groupUserManagement);
        DeleteUser.DeleteUser.MapEndpoint(groupUserManagement);
    }
}
