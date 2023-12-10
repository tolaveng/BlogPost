namespace Core.Application.DTOs
{
    public class Pagable
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
        public string SortDirection { get; set; } = "ASC";

        public int Skip => ((PageNo > 0 ? PageNo : 1) - 1) * PageSize;

    }
}
