using System;
using FluentValidation;
using MediatR;

namespace blogger_clone.Feature.Auth.UserLogin;

public record Command (
    string UsernameOrEmail,
    string Password
) : IRequest<Result>;
public record Result (
    Guid Id,
    string Token
);

public class DataModel : AbstractValidator<Command>
{
    public DataModel()
    {
        RuleFor(t => t.Password)
        .NotEmpty().WithMessage("Password must not be empty");

        RuleFor(t => t.UsernameOrEmail)
        .NotEmpty().WithMessage("Username must not be empty");
    }
}
