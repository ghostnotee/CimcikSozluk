using CimcikSozluk.Common;
using CimcikSozluk.Common.Events.EntryComment;
using CimcikSozluk.Common.Infrastructure;
using MediatR;

namespace CimcikSozluk.Api.Application.Features.Commands.EntryComment.DeleteFav;

public class DeleteEntryCommentFavCommandHandler : IRequestHandler<DeleteEntryCommentFavCommand, bool>
{
    public async Task<bool> Handle(DeleteEntryCommentFavCommand request, CancellationToken cancellationToken)
    {
        QueueFactory.SendMessageToExchange(
            exchangeName: SozlukConstans.FavExchangeName,
            queueName: SozlukConstans.DefaultExchangeType,
            exchangeType: SozlukConstans.DeleteEntryCommentFavQueueName,
            obj: new DeleteEntryCommentFavEvent()
            {
                EntryCommentId = request.EntryCommentId,
                CreatedBy = request.UserId
            }
        );

        return await Task.FromResult(true);
    }
}