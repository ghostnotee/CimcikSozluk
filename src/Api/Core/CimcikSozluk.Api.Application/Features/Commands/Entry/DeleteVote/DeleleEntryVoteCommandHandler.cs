using CimcikSozluk.Common;
using CimcikSozluk.Common.Events.Entry;
using CimcikSozluk.Common.Infrastructure;
using MediatR;

namespace CimcikSozluk.Api.Application.Features.Commands.Entry.DeleteVote;

public class DeleteEntryVoteCommandHandler : IRequestHandler<DeleteEntryVoteCommand, bool>
{
    public async Task<bool> Handle(DeleteEntryVoteCommand request, CancellationToken cancellationToken)
    {
        QueueFactory.SendMessageToExchange(
            exchangeName: SozlukConstans.VoteExchangeName,
            exchangeType: SozlukConstans.DefaultExchangeType,
            queueName: SozlukConstans.DeleteEntryVoteQueueName,
            obj: new DeleteEntryVoteEvent()
            {
                EntryId = request.EntryId,
                CreatedBy = request.UserId
            }
        );

        return await Task.FromResult(true);
    }
}