using CimcikSozluk.Common.Models.RequestModels;
using FluentValidation;
using FluentValidation.Validators;

namespace CimcikSozluk.Api.Application.Features.Commands.User.Login;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(i => i.EmailAddress).NotNull()
            .EmailAddress(EmailValidationMode.AspNetCoreCompatible)
            .WithMessage("{PropertyName} not a valid email address");
        RuleFor(i => i.Password).NotNull()
            .MinimumLength(6).WithMessage("{PropertyName} should at least be {MinLenght} characters");
    }
}