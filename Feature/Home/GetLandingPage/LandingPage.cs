using System;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace blogger_clone.Feature.Home.GetLandingPage;

public class LandingPages
{
    public static void MapEndpoint (RouteGroupBuilder group)
    {
        group.MapGet("/", async()
        =>
        {
            return new RazorComponentResult<LandingPage>();
        })
        .WithName("Get landing page");
    }
}
