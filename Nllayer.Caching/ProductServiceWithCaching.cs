using System.Linq.Expressions;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using NLayer.Core;
using NLayer.Core.DTOs;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWork;
using NLayer.Service.Exception;

namespace Nllayer.Caching;

public class ProductServiceWithCaching:IProductService
{
    private const string CacheKey = "productCache";
    private readonly IMapper _mapper;
    private readonly IMemoryCache _memoryCache;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    public ProductServiceWithCaching(IMapper mapper,IMemoryCache memoryCache,IProductRepository productRepository,IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _memoryCache = memoryCache;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;

        if (!_memoryCache.TryGetValue(CacheKey, out _))
        {
            _memoryCache.Set(CacheKey, _productRepository.GetProductsWithCategoryAsync());
        }
    }
    
    public Task<Product> GetByIdAsync(int id)
    {
        var product = _memoryCache.Get<List<Product>>(CacheKey).FirstOrDefault(x => x.Id == id);
        if (product == null)
        {
            throw new NotFoundException($"{typeof(Product).Name}{id} Not Found");
        }

        return Task.FromResult(product);
        
    }

    public IEnumerable<Product> GetAll()
    {

        return _memoryCache.Get<IEnumerable<Product>>(CacheKey);

    }

    public IEnumerable<Product> Where(Expression<Func<Product, bool>> expression)
    {

        return _memoryCache.Get<List<Product>>(CacheKey).Where(expression.Compile()).AsEnumerable();
    }

    public Task<bool> AnyAsync(Expression<Func<Product, bool>> expression)
    {
        return Task.FromResult(_memoryCache.Get<List<Product>>(CacheKey).Any(expression.Compile()));
    }

    public async Task AddAsync(Product entity)
    {
        await _productRepository.AddAsync(entity);
        
    }

    public async Task AddRangeAsync(IEnumerable<Product> entities)
    {
        await _productRepository.AddRangeAsync(entities);
    }

    public async Task UpdateAsync(Product entity)
    {
        _productRepository.Update(entity);
        await UpdateCache();
    }

    public async Task RemoveAsync(Product entity)
    {
        _productRepository.Remove(entity);
        await _unitOfWork.CommitAsync();
        await UpdateCache();
    }

    public async Task RemoveRangeAsync(IEnumerable<Product> entities)
    {
        _productRepository.RemoveRange(entities);
        await _unitOfWork.CommitAsync();
        await  UpdateCache();

    }

    public Task<CustomResponseDto<List<ProductWithCategory>>> GetProductsWithCategory()
    {
        var products = _memoryCache.Get<List<Product>>(CacheKey);
        var productWithCategory = _mapper.Map<List<ProductWithCategory>>(products);
        return Task.FromResult(CustomResponseDto<List<ProductWithCategory>>.Success(productWithCategory, 200));
    }

    public async Task UpdateCache() => _memoryCache.Set(CacheKey, await _productRepository.GetProductsWithCategoryAsync());
}
