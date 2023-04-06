using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Filter;
using NLayer.Core;
using NLayer.Core.DTOs;
using NLayer.Core.Services;

namespace NLayer.API.Controllers;

public class ProductController:CustomBaseController
{
    private readonly IMapper _mapper;
    private readonly IProductService _productService;

    public ProductController(IMapper mapper,IProductService productService)
    {
        _mapper = mapper;
        _productService = productService;
    }
    [HttpGet("[action]")]
    public async Task<IActionResult> GetProductsWithCategory()
    {
        return CreateActionResult(await _productService.GetProductsWithCategory());
    }
    [HttpGet]
    public  IActionResult All()
    {
        var products = _productService.GetAll();
        return CreateActionResult(CustomResponseDto<List<ProductDto>>.Success(_mapper.Map<List<ProductDto>>(products), 200));
    }

    [HttpPost]
    public async Task<IActionResult> Save(ProductDto productDto)
    {
        var product = _mapper.Map<Product>(productDto);
        await _productService.AddAsync(product);
        return CreateActionResult(CustomResponseDto<ProductDto>.Success(productDto, 201));

    }

    [HttpPut]
    public IActionResult Update(ProductUpdateDto productUpdateDto)
    {
        var product = _mapper.Map<Product>(productUpdateDto);
        _productService.UpdateAsync(product);
        return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
    }

    [ServiceFilter(typeof(NotFoundFilter<>))]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        await  _productService.RemoveAsync(product);
        return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        
    }
    
    [ServiceFilter(typeof(NotFoundFilter<Product>))]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        var productDto = _mapper.Map<ProductDto>(product);
        return CreateActionResult(CustomResponseDto<ProductDto>.Success( productDto,200));
    }

}