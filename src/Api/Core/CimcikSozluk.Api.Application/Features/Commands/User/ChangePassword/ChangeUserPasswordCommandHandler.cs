using CimcikSozluk.Api.Application.Interfaces.Repository;
using CimcikSozluk.Common.Infrastructure;
using CimcikSozluk.Common.Infrastructure.Exceptions;
using CimcikSozluk.Common.Models.RequestModels;
using MediatR;

namespace CimcikSozluk.Api.Application.Features.Commands.User.ChangePassword;

public class ChangeUserPasswordCommandHandler : IRequestHandler<ChangeUserPasswordCommand, bool>
{
    private readonly IUserRepository _userRepository;

    public ChangeUserPasswordCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
    {
        if (!request.UserId.HasValue) throw new ArgumentNullException(nameof(request.UserId));
        var dbUser = await _userRepository.GetByIdAsync(request.UserId.Value);
        if (dbUser is null) throw new DatabaseValidationException("User not found!");
        var oldEncryptedPass = PasswordEncryptor.Encrpt(request.OldPassword);
        if (dbUser.Password != oldEncryptedPass) throw new DatabaseValidationException("Old password wrong!");

        dbUser.Password = PasswordEncryptor.Encrpt(request.NewPassword);
        await _userRepository.UpdateAsync(dbUser);

        return true;
    }
}