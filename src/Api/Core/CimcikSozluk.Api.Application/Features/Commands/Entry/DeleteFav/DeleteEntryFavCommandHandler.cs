using CimcikSozluk.Common;
using CimcikSozluk.Common.Events.Entry;
using CimcikSozluk.Common.Infrastructure;
using MediatR;

namespace CimcikSozluk.Api.Application.Features.Commands.Entry.DeleteFav;

public class DeleteEntryFavCommandHandler : IRequestHandler<DeleteEntryFavCommand, bool>
{
    public async Task<bool> Handle(DeleteEntryFavCommand request, CancellationToken cancellationToken)
    {
        QueueFactory.SendMessageToExchange(
            exchangeName: SozlukConstans.FavExchangeName,
            exchangeType: SozlukConstans.DefaultExchangeType,
            queueName: SozlukConstans.DeleteEntryFavQueueName,
            obj: new DeleteEntryFavEvent()
            {
                EntryId = request.EntryId,
                CreatedBy = request.UserId
            }
        );

        return await Task.FromResult(true);
    }
}