namespace Play.Inventory.Service
{
    public class DTOs
    {
        public record GrantItemDTO(Guid UserId, Guid CatalogItemId, int Quantity);
        public record InventoryItemDTO(Guid CatalogItemId, string Name, string Description, int Quantity, DateTimeOffset AcquiredDate);
        public record CatalogItemDTO(Guid Id, string Name, string Description, decimal Price, DateTimeOffset CreatedDate);
    }
}