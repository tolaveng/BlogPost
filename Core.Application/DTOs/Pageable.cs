namespace Core.Application.DTOs
{
    public class Pageable
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
        public string SortDirection { get; set; } = "ASC";

        public int Skip => ((PageNo > 0 ? PageNo : 1) - 1) * PageSize;

        public Pageable()
        {
            PageNo = 1;
            PageSize = 50;
            SortBy = "";
        }


        public Pageable(int pageNo, int pageSize)
        {
            PageNo = pageNo;
            PageSize = pageSize;
        }

    }
}
