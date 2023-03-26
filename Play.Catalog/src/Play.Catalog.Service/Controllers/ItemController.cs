using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Models;
using Play.Common.Repos;
using static Play.Catalog.Service.DTOs;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly IRepo<Item> _iRepo;
        public ItemController(IRepo<Item> iRepo)
        {
            _iRepo = iRepo;
        }

        [HttpGet]
        public async Task<IEnumerable<ItemDTO>> GetAsync()
        {
            var items = (await _iRepo.GetAllAsync()).Select(item => item.AsDDTO());
            return items;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDTO>> GetSingleAsync(Guid id)
        {
            var item = await _iRepo.GetAsync(id);
            if (item == null)
            {
                return NotFound("This item does not exist.");
            }
            return item.AsDDTO();
        }

        [HttpPost]
        public async Task<ActionResult<ItemDTO>> PostAsync(CreateItemDTO createItemDTO)
        {
            var item = new Item
            {
                Name = createItemDTO.Name,
                Description = createItemDTO.Description,
                Price = createItemDTO.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };
            await _iRepo.CreateAsync(item);
            return CreatedAtAction(nameof(GetSingleAsync), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(Guid id, UpdateItemDTO updateItemDTO)
        {
            var existingItem = await _iRepo.GetAsync(id);
            if (existingItem == null)
            {
                return NotFound();
            }
            existingItem.Name = updateItemDTO.Name;
            existingItem.Description = updateItemDTO.Description;
            existingItem.Price = updateItemDTO.Price;
            await _iRepo.UpdateAsync(existingItem);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var item = await _iRepo.GetAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            await _iRepo.RemoveAsync(item.Id);
            return NoContent();
        }
    }
}