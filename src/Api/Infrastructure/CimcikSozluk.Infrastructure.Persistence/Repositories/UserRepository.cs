using CimcikSozluk.Api.Application.Interfaces.Repository;
using CimcikSozluk.Api.Domain.Models;
using CimcikSozluk.Infrastructure.Persistence.Context;

namespace CimcikSozluk.Infrastructure.Persistence.Repositories;

public class UserRepository: GenericRepository<User>, IUserRepository
{
    public UserRepository(CimcikSozlukContext dbContext, CimcikSozlukContext context) : base(dbContext, context)
    {
    }
}