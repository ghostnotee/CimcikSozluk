using MediatR;

namespace CimcikSozluk.Common.Models.RequestModels;

public class CreateEntryVoteCommand : IRequest<bool>
{
    public CreateEntryVoteCommand(Guid entryId, VoteType voteType, Guid userId)
    {
    }

    public Guid EntryId { get; set; }
    public Guid CreatedBy { get; set; }
    public VoteType VoteType { get; set; }
}