using InventorySystem.Application.DTOs.Request;

namespace InventorySystem.Application.DTOs.Response;

public class PagedInventoryItemResponse
{
    public required IEnumerable<InventoryItemResponse> Items { get; set; }
    public required PageMetadata Metadata { get; set; }
}

