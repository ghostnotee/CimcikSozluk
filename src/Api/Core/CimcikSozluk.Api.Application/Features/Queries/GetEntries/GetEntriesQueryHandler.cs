using AutoMapper;
using AutoMapper.QueryableExtensions;
using CimcikSozluk.Api.Application.Interfaces.Repository;
using CimcikSozluk.Common.Models.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CimcikSozluk.Api.Application.Features.Queries.GetEntries;

public class GetEntriesQueryHandler : IRequestHandler<GetEntriesQuery, List<GetEntriesViewModel>>
{
    private IEntryRepository _entryRepository;
    private IMapper _mapper;

    public GetEntriesQueryHandler(IEntryRepository entryRepository, IMapper mapper)
    {
        _entryRepository = entryRepository;
        _mapper = mapper;
    }

    public async Task<List<GetEntriesViewModel>> Handle(GetEntriesQuery request, CancellationToken cancellationToken)
    {
        var query = _entryRepository.AsQueryable();
        if (request.TodaysEntries)
            query = query.Where(i => i.CreateDate >= DateTime.Now.Date)
                .Where(i => i.CreateDate <= DateTime.Now.AddDays(1).Date);

        query = query.Include(i => i.EntryComments)
            .OrderBy(i => Guid.NewGuid())
            .Take(request.Count);

        return await query.ProjectTo<GetEntriesViewModel>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
    }
}