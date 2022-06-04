using MediatR;

namespace CimcikSozluk.Common.Models.RequestModels;

public class CreateEntryCommentVoteCommand : IRequest<bool>
{
    public CreateEntryCommentVoteCommand(Guid entryCommentId, VoteType voteType, Guid createdBy)
    {
        EntryCommentId = entryCommentId;
        VoteType = voteType;
        CreatedBy = createdBy;
    }

    public CreateEntryCommentVoteCommand()
    {
    }

    public Guid EntryCommentId { get; set; }
    public VoteType VoteType { get; set; }
    public Guid CreatedBy { get; set; }
}