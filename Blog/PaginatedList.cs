namespace Blog
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        
        public PaginatedList(List<T> page, int listSize, int pageSize, int pageIndex)
        {
            PageIndex = pageIndex;
            TotalPages = listSize / pageSize;

            this.AddRange(page);
        }

        public bool HasPreviousPage
        {
            get { return (PageIndex > 1); }
        }
        public bool HasNextPage
        {
            get { return (PageIndex < TotalPages); }
        }
        
        public static async Task<PaginatedList<T>> CreateAsync(List<T> list, int pageSize, int pageIndex)
        {
            var listSize = list.Count;
            var page = list
                .Skip(pageSize * (pageIndex - 1))
                .Take(pageSize)
                .ToList();
                
            return new PaginatedList<T>(page, listSize, pageSize, pageIndex);
        }
    }
}
