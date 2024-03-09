using X.PagedList;

namespace ICHI_API.Helpers
{
  public class PagedResult<T>
  {
    public IEnumerable<T> Items { get; set; }
    public int TotalCount { get; set; } // tổng số bản ghi
    public int PageCount { get; set; } // tổng số trang
    public int CurrentPage { get; set; } // trang hiện tại
    public int PageSize { get; set; } // số bản ghi trên mỗi trang
    public bool HasPreviousPage { get; set; } // có trang trước không
    public bool HasNextPage { get; set; } // có trang sau không
    public static PagedResult<T> CreatePagedResult<T>(IQueryable<T> query, int pageNumber, int pageSize)
    {
      var pagedList = query.ToPagedList(pageNumber, pageSize);
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
    //public static async Task<PagedResult<T>> CreatePagedResultAsync<T>(IQueryable<T> query, int pageNumber, int pageSize)
    //{
    //    var pagedList = await query.ToPagedListAsync(pageNumber, pageSize);
    //    return new PagedResult<T>
    //    {
    //        Items = pagedList.ToList(),
    //        TotalCount = pagedList.TotalItemCount,
    //        PageCount = pagedList.PageCount,
    //        CurrentPage = pagedList.PageNumber,
    //        PageSize = pagedList.PageSize,
    //        HasPreviousPage = pagedList.HasPreviousPage,
    //        HasNextPage = pagedList.HasNextPage
    //    };
    //}
  }

}
