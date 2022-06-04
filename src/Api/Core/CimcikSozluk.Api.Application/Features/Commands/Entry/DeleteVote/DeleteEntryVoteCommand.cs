using MediatR;

namespace CimcikSozluk.Api.Application.Features.Commands.Entry.DeleteVote;

public abstract class DeleteEntryVoteCommand : IRequest<bool>
{
    public DeleteEntryVoteCommand(Guid entryId, Guid userId)
    {
        EntryId = entryId;
        UserId = userId;
    }

    public Guid EntryId { get; set; }
    public Guid UserId { get; set; }
}