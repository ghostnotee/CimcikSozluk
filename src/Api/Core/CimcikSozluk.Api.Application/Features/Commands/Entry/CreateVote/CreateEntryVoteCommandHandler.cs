using CimcikSozluk.Common;
using CimcikSozluk.Common.Events.Entry;
using CimcikSozluk.Common.Infrastructure;
using CimcikSozluk.Common.Models.RequestModels;
using MediatR;

namespace CimcikSozluk.Api.Application.Features.Commands.Entry.CreateVote;

public class CreateEntryVoteCommandHandler : IRequestHandler<CreateEntryVoteCommand, bool>
{
    public async Task<bool> Handle(CreateEntryVoteCommand request, CancellationToken cancellationToken)
    {
        QueueFactory.SendMessageToExchange(
            exchangeName: SozlukConstans.VoteExchangeName,
            exchangeType: SozlukConstans.DefaultExchangeType,
            queueName: SozlukConstans.CreateEntryVoteQueueName,
            obj: new CreateEntryVoteEvent()
            {
                EntryId = request.EntryId,
                CreatedBy = request.CreatedBy,
                VoteType = request.VoteType
            });

        return await Task.FromResult(true);
    }
}