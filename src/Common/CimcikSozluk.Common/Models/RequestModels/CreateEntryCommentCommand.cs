namespace CimcikSozluk.Common.Models.RequestModels;

public class CreateEntryCommentCommand
{
    public CreateEntryCommentCommand(Guid entryId, string content, Guid createdById)
    {
        EntryId = entryId;
        Content = content;
        CreatedById = createdById;
    }

    public CreateEntryCommentCommand()
    {
    }

    public Guid? EntryId { get; set; }
    public string Content { get; set; }
    public Guid? CreatedById { get; set; }
}