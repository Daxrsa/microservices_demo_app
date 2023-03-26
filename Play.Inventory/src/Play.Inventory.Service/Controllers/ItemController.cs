using Microsoft.AspNetCore.Mvc;
using Play.Common.Repos;
using Play.Inventory.Service.clients;
using Play.Inventory.Service.Models;
using static Play.Inventory.Service.DTOs;

namespace Play.Inventory.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly IRepo<InventoryItem> _itemRepo;
        private readonly CatalogClient _catalogClient;
        public ItemController(IRepo<InventoryItem> itemRepo,CatalogClient catalogClient)
        {
            _itemRepo = itemRepo;
            _catalogClient = catalogClient;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryItemDTO>>> GetAsync(Guid userId)
        {
            if(userId == Guid.Empty)
            {
                return BadRequest();
            }

            var catalogItems = await _catalogClient.GetCatalogItemsAsync();
            var inventoryItemEntities = await _itemRepo.GetAllAsync(item => item.UserId == userId);
            var inventoryItemDTOs = inventoryItemEntities.Select(inventoryItem => {
                var catalogItem = catalogItems.Single(catalogItem => catalogItem.Id == inventoryItem.CatalogItemId);
                return inventoryItem.AsDTO(catalogItem.Name, catalogItem.Description);
            });
            return Ok(inventoryItemDTOs);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(GrantItemDTO grantItemDTO)
        {
            var inventoryItem = await _itemRepo.GetAsync(
                item => item.UserId == grantItemDTO.UserId && item.CatalogItemId == grantItemDTO.CatalogItemId);

            if(inventoryItem == null)
            {
                inventoryItem = new InventoryItem{
                    CatalogItemId = grantItemDTO.CatalogItemId,
                    UserId = grantItemDTO.UserId,
                    Quantity = grantItemDTO.Quantity,
                    AcquiredDate = DateTimeOffset.UtcNow
                };
                await _itemRepo.CreateAsync(inventoryItem);
            }
            else{
                inventoryItem.Quantity += grantItemDTO.Quantity;
                await _itemRepo.UpdateAsync(inventoryItem);
            }
            return Ok(inventoryItem);
        }

        
    }
}