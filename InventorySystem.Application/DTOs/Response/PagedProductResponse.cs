namespace InventorySystem.Application.DTOs.Response;

public class PagedProductResponse
{
    public required IEnumerable<ProductResponse> Items { get; set; }
    public required PageMetadata Metadata { get; set; }
}

public class PageMetadata
{
    public int TotalCount { get; set; }
    public int PageSize { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public bool HasNext { get; set; }
    public bool HasPrevious { get; set; }
}