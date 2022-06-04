using CimcikSozluk.Common;
using CimcikSozluk.Common.Events.EntryComment;
using CimcikSozluk.Common.Infrastructure;
using CimcikSozluk.Common.Models.RequestModels;
using MediatR;

namespace CimcikSozluk.Api.Application.Features.Commands.EntryComment.CreateVote;

public class CreateEntryCommentVoteCommandHandler : IRequestHandler<CreateEntryCommentVoteCommand, bool>
{
    public async Task<bool> Handle(CreateEntryCommentVoteCommand request, CancellationToken cancellationToken)
    {
        QueueFactory.SendMessageToExchange(
            exchangeName: SozlukConstans.VoteExchangeName,
            exchangeType: SozlukConstans.DefaultExchangeType,
            queueName: SozlukConstans.CreateEntryCommentVoteQueueName,
            obj: new CreateEntryCommentVoteEvent()
            {
                EntryCommentId = request.EntryCommentId,
                VoteType = request.VoteType,
                CreatedBy = request.CreatedBy
            }
        );

        return await Task.FromResult(true);
    }
}