using Play.Inventory.Service.Models;
using static Play.Inventory.Service.DTOs;

namespace Play.Inventory.Service
{
    public static class Extensions
    {
        public static InventoryItemDTO AsDTO(this InventoryItem item, string Name, string Description)
        {
            return new InventoryItemDTO(item.CatalogItemId, Name, Description, item.Quantity, item.AcquiredDate);
        }
    }
}