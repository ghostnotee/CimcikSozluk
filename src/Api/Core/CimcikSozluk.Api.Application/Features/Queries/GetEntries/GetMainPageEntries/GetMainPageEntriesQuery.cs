using AutoMapper;
using CimcikSozluk.Common.Models.Page;
using CimcikSozluk.Common.Models.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CimcikSozluk.Api.Application.Features.Queries.GetEntries.GetMainPageEntries;

public class GetMainPageEntriesQuery : BasePagedQuery, IRequest<PagedViewModel<GetEntryDetailViewModel>>
{
    public Guid? UserId { get; set; }

    public GetMainPageEntriesQuery(int page, int pageSize, Guid? userId) : base(page, pageSize)
    {
        UserId = userId;
    }
}