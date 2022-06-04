using MediatR;

namespace CimcikSozluk.Common.Events.Entry;

public class CreateEntryCommentCommand : IRequest<Guid>
{
    public Guid EntryId { get; set; }
    public string Content { get; set; }
    public Guid CreatedBy { get; set; }
}