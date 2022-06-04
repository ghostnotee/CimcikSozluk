using CimcikSozluk.Common;
using CimcikSozluk.Common.Events.EntryComment;
using CimcikSozluk.Common.Infrastructure;
using MediatR;

namespace CimcikSozluk.Api.Application.Features.Commands.EntryComment.DeleteVote;

public class DeleteEntryCommentVoteHandler : IRequestHandler<DeleteEntryCommentVoteCommand, bool>
{
    public async Task<bool> Handle(DeleteEntryCommentVoteCommand request, CancellationToken cancellationToken)
    {
        QueueFactory.SendMessageToExchange(
            exchangeName: SozlukConstans.FavExchangeName,
            queueName: SozlukConstans.DefaultExchangeType,
            exchangeType: SozlukConstans.DeleteEntryCommentVoteQueueName,
            obj: new DeleteEntryCommentVoteEvent()
            {
                EntryCommentId = request.EntryCommentId,
                CreatedBy = request.UserId
            }
        );

        return await Task.FromResult(true);
    }
}