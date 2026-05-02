using System;
using blogger_clone.Feature.Home.GetLandingPage;
using Carter;

namespace blogger_clone.Feature.Home;

public class HomeApi : ICarterModule
{
    public void AddRoutes (IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/home")
        .WithName("Home")
        .AllowAnonymous();

        LandingPages.MapEndpoint(group);
    }
}
