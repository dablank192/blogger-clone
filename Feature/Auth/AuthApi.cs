using System;
using Carter;


namespace blogger_clone.Feature.Auth;

public class AuthApi : ICarterModule
{
    public void AddRoutes (IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/auth")
        .WithTags("Auth");

        UserRegister.UserRegister.MapEndpoint(group);
        UserLogin.UserLogin.MapEndpoint(group);
    }
}
