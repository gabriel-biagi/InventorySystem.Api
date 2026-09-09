namespace InventorySystem.Domain.Pagination;

public class PagedList<T> : List<T> where T : class
{
    public int Currentpage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    
    public bool  HasPrevious => Currentpage > 1;
    public bool HasNext => Currentpage < TotalPages;

    public PagedList(List<T> items, int count, int pageNumber, int pageSize)
    {
        TotalCount = count;
        PageSize = pageSize;
        Currentpage = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        
        AddRange(items);
    }

    public static PagedList<T> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize)
    {
        var count = source.Count();
        var items = source
            .Skip((pageNumber - 1) * pageSize).
            Take(pageSize)
            .ToList();
        return new PagedList<T>(items, count, pageNumber, pageSize);
    }
}