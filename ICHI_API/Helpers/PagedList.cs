using ICHI_API.Helpers;
using X.PagedList;

namespace API.Helpers
{
  public class PagedList<T> : List<T>
  {
    public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
    {
      CurrentPage = pageNumber;
      TotalPages = (int)Math.Ceiling(count / (double)pageSize);
      PageSize = pageSize;
      TotalCount = count;
      AddRange(items);
    }

    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }

    public static async Task<PagedResult<T>> CreateAsync(IQueryable<T> query, int pageNumber, int pageSize)
    {
      var pagedList = await query.ToPagedListAsync(pageNumber, pageSize);
      return new PagedResult<T>
      {
        Items = pagedList.ToList(),
        TotalCount = pagedList.TotalItemCount,
        PageCount = pagedList.PageCount,
        CurrentPage = pagedList.PageNumber,
        PageSize = pagedList.PageSize,
        HasPreviousPage = pagedList.HasPreviousPage,
        HasNextPage = pagedList.HasNextPage
      };
    }
  }
}
