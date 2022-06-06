using CimcikSozluk.Api.Application.Interfaces.Repository;
using CimcikSozluk.Common.Infrastructure.Extensions;
using CimcikSozluk.Common.Models;
using CimcikSozluk.Common.Models.Page;
using CimcikSozluk.Common.Models.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CimcikSozluk.Api.Application.Features.Queries.GetEntryComments;

public class
    GetEntryCommentsQueryHandler : IRequestHandler<GetEntryCommentsQuery, PagedViewModel<GetEntryCommentsViewModel>>
{
    private readonly IEntryCommentRepository _entryCommentRepository;

    public GetEntryCommentsQueryHandler(IEntryCommentRepository entryCommentRepository)
    {
        _entryCommentRepository = entryCommentRepository;
    }


    public async Task<PagedViewModel<GetEntryCommentsViewModel>> Handle(GetEntryCommentsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _entryCommentRepository.AsQueryable();
        query = query.Include(i => i.EntryCommentFavorites)
            .Include(i => i.CreatedBy)
            .Include(i => i.EntryCommentVotes)
            .Where(i => i.EntryId == request.EntryId);

        var list = query.Select(i => new GetEntryCommentsViewModel()
        {
            Id = i.Id,
            Content = i.Content,
            IsFavorited = request.UserId.HasValue && i.EntryCommentFavorites.Any(j => j.CreatedById == request.UserId),
            FavoritedCount = i.EntryCommentFavorites.Count,
            CreatedDate = i.CreateDate,
            CreatedByUserName = i.CreatedBy.UserName,
            VoteType = request.UserId.HasValue && i.EntryCommentVotes.Any(j => j.CreatedById == request.UserId)
                ? i.EntryCommentVotes.FirstOrDefault(j => j.CreatedById == request.UserId).VoteType
                : VoteType.None
        });

        var entries = await list.GetPaged(request.Page, request.PageSize);

        return entries;
    }
}