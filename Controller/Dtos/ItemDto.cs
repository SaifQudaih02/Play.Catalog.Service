namespace Play.Catalog.Service.Controller.Dtos;

public record ItemDto(Guid Id, string Name, string Description, decimal Price, DateTimeOffset DateTimeOffset)
{
}