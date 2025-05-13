namespace Shader.Helpers
{
    public class PagedResponse<T>
    {
        public int CurrentPage { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public List<T> Data { get; set; }
    }
}
