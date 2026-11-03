using System.ComponentModel.DataAnnotations;

namespace Play.Catalog.Service.Controller.Dtos;

public record UpdateItemDto([Required] string Name, string Description, [Range(0,1000)] decimal Price);