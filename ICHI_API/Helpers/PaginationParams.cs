using ICHI_CORE.Helpers;

namespace API.Helpers
{
    public class PaginationParams
    {
        public int PageNumber { get; set; } = AppSettings.PageNumber;
        public int PageSize { get; set; } = AppSettings.PageSize;
        public string Search { get; set; } = string.Empty;
        public string SortBy { get; set; } = AppSettings.SortBy;
        public string SortDirection { get; set; } = AppSettings.SortDirection;
    }
}
