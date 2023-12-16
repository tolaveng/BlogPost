namespace Core.Application.DTOs
{
    public class Pagination<T>
    {
        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
        public int TotalPages { get; set; }
        public long Count { get; set; }
        public bool HasNext { get; set; }
    }
}
