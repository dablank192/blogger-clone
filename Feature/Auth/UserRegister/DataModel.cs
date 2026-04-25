using System;
using FluentValidation;
using MediatR;

namespace blogger_clone.Feature.Auth.UserRegister;


public record Command (
    string Email,
    string Password
) : IRequest<Result>;

public record Result (
    Guid Id,
    string Message
);


public class Validator : AbstractValidator<Command>
{
    public Validator()
    {
        RuleFor(t => t.Email)
        .EmailAddress().WithMessage("Invalid Email format")
        .NotEmpty().WithMessage("Email field must not be empty");

        RuleFor(t => t.Password)
        .NotEmpty().WithMessage("Password must not be empty")
        .MinimumLength(8).WithMessage("Password must be more than 8 character");
    }
}

