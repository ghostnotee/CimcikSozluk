using AutoMapper;
using CimcikSozluk.Api.Application.Interfaces.Repository;
using CimcikSozluk.Common;
using CimcikSozluk.Common.Events.User;
using CimcikSozluk.Common.Infrastructure;
using CimcikSozluk.Common.Infrastructure.Exceptions;
using CimcikSozluk.Common.Models.RequestModels;
using MediatR;

namespace CimcikSozluk.Api.Application.Features.Commands.User.Create;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public CreateUserCommandHandler(IMapper mapper, IUserRepository userRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var existsUser = await _userRepository.GetSingleAsync(i => i.EmailAddress == request.EmailAddress);
        if (existsUser is not null)
            throw new DatabaseValidationException("User already exists!");
        var dbUser = _mapper.Map<Domain.Models.User>(request);
        var rows = await _userRepository.AddAsync(dbUser);

        if (rows > 0)
        {
            var @event = new UserEmailChangedEvent()
            {
                OldEmailAddress = null,
                NewEmailAddress = request.EmailAddress
            };

            QueueFactory.SendMessageExchange(exchangeName: SozlukConstans.UserExchangeName,
                exchangeType: SozlukConstans.DefaultExchangeType, queueName: SozlukConstans.UserEmailChangedQueueName,
                obj: @event);
        }

        return dbUser.Id;
    }
}