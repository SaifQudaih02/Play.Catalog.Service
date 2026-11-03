using System.ComponentModel.DataAnnotations;

namespace Play.Catalog.Service.Controller.Dtos;

public record CreateItemDto(
    [Required]
    string Name,
    string Description, 
    [Range(0,1000)]
    decimal Price
    );