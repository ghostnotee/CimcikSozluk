using AutoMapper;
using CimcikSozluk.Api.Application.Interfaces.Repository;
using CimcikSozluk.Common;
using CimcikSozluk.Common.Events.User;
using CimcikSozluk.Common.Infrastructure;
using CimcikSozluk.Common.Infrastructure.Exceptions;
using CimcikSozluk.Common.Models.RequestModels;
using MediatR;

namespace CimcikSozluk.Api.Application.Features.Commands.User.Update;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UpdateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var dbuser = await _userRepository.GetByIdAsync(request.Id);

        if (dbuser is null)
            throw new DatabaseValidationException("User not found!");

        var dbEmailAddress = dbuser.EmailAddress;
        var emailChanged = string.CompareOrdinal(dbEmailAddress, request.EmailAddress) != 0;

        _mapper.Map(request, dbuser);
        var rows = await _userRepository.UpdateAsync(dbuser);

        if (emailChanged && rows > 0)
        {
            var @event = new UserEmailChangedEvent()
            {
                OldEmailAddress = null,
                NewEmailAddress = request.EmailAddress
            };

            QueueFactory.SendMessageToExchange(exchangeName: SozlukConstans.UserExchangeName,
                exchangeType: SozlukConstans.DefaultExchangeType, queueName: SozlukConstans.UserEmailChangedQueueName,
                obj: @event);
            
            dbuser.EmailConfirmed = false;
            await _userRepository.UpdateAsync(dbuser);
        }

        return dbuser.Id;
    }
}