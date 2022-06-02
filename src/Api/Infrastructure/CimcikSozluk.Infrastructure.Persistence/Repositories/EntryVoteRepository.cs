using CimcikSozluk.Api.Application.Interfaces.Repository;
using CimcikSozluk.Api.Domain.Models;
using CimcikSozluk.Infrastructure.Persistence.Context;

namespace CimcikSozluk.Infrastructure.Persistence.Repositories;

public class EntryVoteRepository : GenericRepository<EntryVote>, IEntryVoteRepository
{
    public EntryVoteRepository(CimcikSozlukContext dbContext, CimcikSozlukContext context) : base(dbContext, context)
    {
    }
}