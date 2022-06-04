using MediatR;

namespace CimcikSozluk.Common.Models.RequestModels;

public class CreateEntryCommand : IRequest<Guid>
{
    public CreateEntryCommand(string subject, string content, Guid createdById)
    {
        Subject = subject;
        Content = content;
        CreatedById = createdById;
    }

    public CreateEntryCommand()
    {
    }

    public string Subject { get; set; }
    public string Content { get; set; }
    public Guid? CreatedById { get; set; }
}