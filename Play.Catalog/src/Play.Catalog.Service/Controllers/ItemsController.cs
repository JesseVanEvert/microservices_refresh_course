using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private static readonly List<ItemDto> items = new()
        {
            new ItemDto(Guid.NewGuid(), "Potion", "Restores 10 HP", 9, DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Antidote", "Cures poison", 5, DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Bronze Sword", "Deals 5 damage", 20, DateTimeOffset.UtcNow)
        };

        [HttpGet]
        public IEnumerable<ItemDto> Get()
        {
            return items;
        }

        [HttpGet("{id}")]
        public ItemDto GetById(Guid id)
        {
            var item = items.SingleOrDefault(item => item.Id == id) ?? throw new Exception("Item not found");
            return item;
        }

        [HttpPost]
        public ActionResult<ItemDto> CreateItem(CreateItemDto itemDto)
        {
            var item = new ItemDto(Guid.NewGuid(), itemDto.Name, itemDto.Description, itemDto.Price, DateTimeOffset.UtcNow);
            items.Add(item);
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateItem(Guid id, UpdateItemDto itemDto)
        {
            var existingItem = items.SingleOrDefault(item => item.Id == id);
            if (existingItem is null)
            {
                return NotFound();
            }

            var updatedItem = existingItem with
            {
                Name = itemDto.Name,
                Description = itemDto.Description,
                Price = itemDto.Price
            };

            var index = items.FindIndex(item => item.Id == id);
            items[index] = updatedItem;

            return NoContent();
        }

        
    }
}