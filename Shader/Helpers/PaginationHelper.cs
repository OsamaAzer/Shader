namespace Shader.Helpers
{
    public static class PaginationHelper
    {
        public static PagedResponse<T> CreatePagedResponse<T>(this IEnumerable<T> source, int pageNumber, int pageSize) where T : class
        {
            //if (pageNumber < 1)
            //{
            //    pageNumber = 1;
            //}
            //if (pageSize < 1)
            //{
            //    pageSize = 10; // Default page size
            //}
            //if (source == null || !source.Any())
            //{
            //    return new PagedResponse<T>
            //    {
            //        CurrentPage = pageNumber,
            //        PageNumber = pageNumber,
            //        PageSize = pageSize,
            //        TotalCount = 0,
            //        Data = new List<T>()
            //    };
            //}
            
            var totalCount = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PagedResponse<T>
            {
                CurrentPage = pageNumber,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Data = items
            };
        }
    }
}
