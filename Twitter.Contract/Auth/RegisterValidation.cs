using System.Data;
using FluentValidation;

namespace Twitter.Contract.Auth;

public class RegisterValidation : AbstractValidator<RegisterDto>
{
    public RegisterValidation()
    {
        RuleFor(u => u.Email).NotEmpty().EmailAddress();
        RuleFor(u => u.FirstName).NotEmpty();
        RuleFor(u => u.LastName).NotEmpty();
        RuleFor(u => u.Password).NotEmpty();
    }
}