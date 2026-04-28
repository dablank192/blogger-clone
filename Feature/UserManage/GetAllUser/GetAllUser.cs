using System;
using blogger_clone.Dto;
using blogger_clone.Extension;
using blogger_clone.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace blogger_clone.Feature.UserManage.GetAllUser;

public record Query() : IRequest<Result>;
public record Result(IEnumerable<UserDto> Data);

public class GetAllUser (
    AppDbContext dbContext
) : IRequestHandler<Query, Result>

{
    public static void MapEndPoint(RouteGroupBuilder group)
    {
        group.MapGet("/", async(
            ISender sender
        ) =>
        {
            return Results.Ok(await sender.Send(new Query()));
        })
        .WithName("Get All User (Admim)")
        .Produces<Result>(StatusCodes.Status200OK);
    }

    public async Task<Result> Handle (Query req, CancellationToken ct)
    {
        var user = await dbContext.User.Select(
            t => new UserDto(
                UserId: t.Id,
                Username: t.Username,
                Email: t.Email
            )
        ).ToListAsync(ct);

        var response = new Result(user);

        return response;
    }
}
