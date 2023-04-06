namespace NLayer.Core.DTOs;

public class ProductWithCategory:ProductDto
{
    public CategoryDto Category { get; set; }
}