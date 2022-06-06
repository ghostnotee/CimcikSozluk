using CimcikSozluk.Common.Models.Page;
using Microsoft.EntityFrameworkCore;

namespace CimcikSozluk.Common.Infrastructure.Extensions;

public static class PagingExtension
{
    public static async Task<PagedViewModel<T>> GetPaged<T>(this IQueryable<T> query, int currentPage, int pageSize)
        where T : class
    {
        var count = await query.CountAsync();
        Page paging = new Page(currentPage, pageSize, count);
        var data = await query.Skip(paging.Skip).Take(paging.PageSize).AsNoTracking().ToListAsync();
        var result = new PagedViewModel<T>(data, paging);

        return result;
    }
}