using Play.Catalog.Service.Controller.Dtos;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service;

public class Extensions
{
    public static ItemDto AsDto(Item item)
    {
        return new ItemDto(item.Id, item.Name, item.Description, item.Price, item.CreatedAt);
    }
}