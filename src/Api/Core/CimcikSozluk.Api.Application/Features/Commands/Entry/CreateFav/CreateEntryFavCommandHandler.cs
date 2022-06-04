using CimcikSozluk.Common;
using CimcikSozluk.Common.Events.Entry;
using CimcikSozluk.Common.Infrastructure;
using MediatR;

namespace CimcikSozluk.Api.Application.Features.Commands.Entry.CreateFav;

public class CreateEntryFavCommandHandler : IRequestHandler<CreateEntryFavCommand, bool>
{
    public async Task<bool> Handle(CreateEntryFavCommand request, CancellationToken cancellationToken)
    {
        QueueFactory.SendMessageToExchange(
            exchangeName: SozlukConstans.FavExchangeName,
            exchangeType: SozlukConstans.DefaultExchangeType,
            queueName: SozlukConstans.CreateEntryFavQueueName, obj: new CreateEntryFavEvent()
            {
                EntryId = request.EntryId.Value,
                MyProperty = request.UserId.Value
            });

        return await Task.FromResult(true);
    }
}