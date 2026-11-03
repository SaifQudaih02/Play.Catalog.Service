using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Controller.Dtos;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Repository;
using Play.Catalog.Service.Repository.Interface;

namespace Play.Catalog.Service.Controller;


[ApiController]
[Route("api/items")]
public class ItemsController: ControllerBase
{

    private readonly IItemsRepository _itemsRepository;

    public ItemsController(IItemsRepository itemsRepository)
    {
        _itemsRepository = itemsRepository;
    }

    [HttpGet]
    public async Task<IEnumerable<ItemDto>> GetAsync()
    {
        var items = await _itemsRepository.GetAllAsync();
        return items.Select(item => Extensions.AsDto(item));
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
    {
        var item = await _itemsRepository.GetAsync(id);
        if (item == null)
        {
            return NotFound();
        }
        
        return Extensions.AsDto(item);
    }
    
    [HttpPost]
    public  async Task<ActionResult<ItemDto>> PostAsync(CreateItemDto createItemDto)
    {
        var item = new Item
        {
            Name= createItemDto.Name,
            Description= createItemDto.Description,
            Price = createItemDto.Price,
            CreatedAt = DateTimeOffset.UtcNow
        };
        
        await _itemsRepository.CreateAsync(item);
        return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsync(Guid id, UpdateItemDto updateItemDto)
    {
        var existingItem = await _itemsRepository.GetAsync(id);

        if (existingItem == null) return NoContent();

        existingItem.Name = updateItemDto.Name;
        existingItem.Description = updateItemDto.Description;
        existingItem.Price = updateItemDto.Price;
        await _itemsRepository.UpdateAsync(existingItem);
        
        return NoContent();
    }

    
    [HttpDelete("{id}")]
    public  async Task<IActionResult> DeleteAsync(Guid id)
    {

        var existingItem = _itemsRepository.GetAsync(id);
        if (existingItem == null) return NotFound();
        
        await _itemsRepository.RemoveAsync(id);
        
        return NoContent();
    }
}