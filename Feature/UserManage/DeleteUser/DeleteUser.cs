using System;
using blogger_clone.Exception.Auth;
using blogger_clone.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace blogger_clone.Feature.UserManage.DeleteUser;

public record Command (Guid UserId) : IRequest;


public class DeleteUser (
    AppDbContext dbContext
) : IRequestHandler<Command>

{
    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapDelete("/{UserId}", async(ISender sender, Guid UserId)
        =>
        {
            await sender.Send(new Command(UserId));

            return Results.NoContent();
        })
        .WithName("Delete A User")
        .Produces(StatusCodes.Status204NoContent);
    }

    public async Task Handle (Command req, CancellationToken ct)
    {
        var user = await dbContext.User.FirstOrDefaultAsync(t => t.Id == req.UserId, ct)
        ?? throw new UserNotFoundException(req.UserId);

        dbContext.User.Remove(user);
        await dbContext.SaveChangesAsync(ct);
    }
}
