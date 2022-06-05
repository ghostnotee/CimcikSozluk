using CimcikSozluk.Api.Application.Interfaces.Repository;
using CimcikSozluk.Common.Infrastructure.Extensions;
using CimcikSozluk.Common.Models;
using CimcikSozluk.Common.Models.Page;
using CimcikSozluk.Common.Models.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CimcikSozluk.Api.Application.Features.Queries.GetEntries.GetMainPageEntries;

public class
    GetMainPAgeEntriesQueryHandler : IRequestHandler<GetMainPageEntriesQuery, PagedViewModel<GetEntryDetailViewModel>>
{
    private readonly IEntryRepository _entryRepository;

    public GetMainPAgeEntriesQueryHandler(IEntryRepository entryRepository)
    {
        _entryRepository = entryRepository;
    }

    public async Task<PagedViewModel<GetEntryDetailViewModel>> Handle(GetMainPageEntriesQuery request,
        CancellationToken cancellationToken)
    {
        var query = _entryRepository.AsQueryable();
        query = query.Include(i => i.EntryFavorites)
            .Include(i => i.CreatedBy)
            .Include(i => i.EntryVotes);

        var list = query.Select(i => new GetEntryDetailViewModel()
        {
            Id = i.Id,
            Subject = i.Subject,
            Content = i.Content,
            IsFavorited = request.UserId.HasValue && i.EntryFavorites.Any(j => j.CreatedById == request.UserId),
            FavoritedCount = i.EntryFavorites.Count,
            CreatedDate = i.CreateDate,
            CreatedByUserName = i.CreatedBy.UserName,
            VoteType = request.UserId.HasValue && i.EntryVotes.Any(j => j.CreatedById == request.UserId)
                ? i.EntryVotes.FirstOrDefault(j => j.CreatedById == request.UserId).VoteType
                : VoteType.None
        });

        var entries = await list.GetPaged(request.Page, request.PageSize);

        return new PagedViewModel<GetEntryDetailViewModel>(entries.Results, entries.PageInfo);
    }
}