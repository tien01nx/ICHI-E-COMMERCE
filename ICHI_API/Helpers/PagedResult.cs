using X.PagedList;

namespace ICHI_API.Helpers
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }

        public static async Task<PagedResult<T>> CreatePagedResultAsync<T>(IQueryable<T> query, int pageNumber, int pageSize)
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
