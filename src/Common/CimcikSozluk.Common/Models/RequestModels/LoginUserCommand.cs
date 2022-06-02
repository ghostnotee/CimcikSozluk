using CimcikSozluk.Common.Models.Queries;
using MediatR;

namespace CimcikSozluk.Common.Models.RequestModels;

public class LoginUserCommand : IRequest<LoginUserViewModel>
{
    public LoginUserCommand(string emailAddress, string password)
    {
        EmailAddress = emailAddress;
        Password = password;
    }

    public LoginUserCommand()
    {
    }

    public string EmailAddress { get; set; }
    public string Password { get; set; }
}