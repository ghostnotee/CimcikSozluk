using CimcikSozluk.Common;
using CimcikSozluk.Common.Events.EntryComment;
using CimcikSozluk.Common.Infrastructure;
using MediatR;

namespace CimcikSozluk.Api.Application.Features.Commands.EntryCommand.CreateFav;

public class CreateEntryCommentFavCommandHandler : IRequestHandler<CreateEntryCommentFavCommand, bool>
{
    public async Task<bool> Handle(CreateEntryCommentFavCommand request, CancellationToken cancellationToken)
    {
        QueueFactory.SendMessageToExchange(exchangeName: SozlukConstans.FavExchangeName,
            exchangeType: SozlukConstans.DefaultExchangeType, SozlukConstans.CreateEntryCommentFavQueueName,
            obj: new CreateEntryCommentFavEvent()
            {
                EntryCommentId = request.EntryCommentId,
                CreatedBy = request.UserId
            });

        return await Task.FromResult(true);
    }
}