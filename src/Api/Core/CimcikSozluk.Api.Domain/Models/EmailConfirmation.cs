namespace CimcikSozluk.Api.Domain.Models;

public class EmailConfirmation : BaseEntity
{
    public string OldEmailAdress { get; set; }
    public string NewEmailAddress { get; set; }
}