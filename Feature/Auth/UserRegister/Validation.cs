using System;
using FluentValidation;

namespace blogger_clone.Feature.Auth.UserRegister;

public class Validation : AbstractValidator<RequestModel>
{
    public Validation()
    {
        RuleFor(t => t.Email)
        .EmailAddress().WithMessage("Invalid Email format")
        .NotEmpty().WithMessage("Email field must not be empty");

        RuleFor(t => t.Password)
        .NotEmpty().WithMessage("Password must not be empty")
        .MinimumLength(8).WithMessage("Password must be more than 8 character");
    }
}
