using FluentValidation;
using NLayer.Core.DTOs;

namespace NLayer.Service.Validator;

public class ProductDtoValidator:AbstractValidator<ProductDto>
{
    public ProductDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().NotNull().WithMessage("{PropertyName}} is required");
        RuleFor(x => x.Price).InclusiveBetween(1, Int32.MaxValue).WithMessage("{PropertyName} must greatet than 0");
        RuleFor(x => x.CategoryId).InclusiveBetween(1, int.MaxValue).WithMessage("{PropertyName} must greater than 0");
    }
    
} 