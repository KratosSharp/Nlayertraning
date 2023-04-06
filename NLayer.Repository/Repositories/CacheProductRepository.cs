using System.Linq.Expressions;
using Newtonsoft.Json;
using NLayer.Core;
using NLayer.Core.Repositories;
using NLayer.Repository.Redis;

namespace NLayer.Repository.Repositories;

public class CacheProductRepository:IProductRepository
{
    private readonly ProductRepository _productRepository;
    private readonly RedisConnection _redisConnection;

    public CacheProductRepository(ProductRepository productRepository,RedisConnection redisConnection)
    {
        _productRepository = productRepository;
        _redisConnection = redisConnection;
    }
    JsonSerializerSettings settings = new JsonSerializerSettings
    {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    };
    public async Task<Product> GetByIdAsync(int id)
    {
        string cachekey = $"product {id}";
        var db = _redisConnection.GetDatabase(0);
        var productJson = await db.StringGetAsync(cachekey);

        if (productJson.IsNullOrEmpty)
        {
            var product = await _productRepository.GetByIdAsync(id);
            productJson = JsonConvert.SerializeObject(product);
            await db.StringSetAsync(cachekey, productJson, TimeSpan.FromMinutes(1));
        }

        return JsonConvert.DeserializeObject<Product>(productJson);
    }

    public IQueryable<Product> GetAll()
    {
        return  _productRepository.GetAll();
    }

    public IQueryable<Product> Where(Expression<Func<Product, bool>> expression)
    {
        return _productRepository.Where(expression);
    }

    public async Task<bool> AnyAsync(Expression<Func<Product, bool>> expression)
    {
        return await _productRepository.AnyAsync(expression);
    }

    public async Task AddAsync(Product entity)
    {
        await _productRepository.AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<Product> entities)
    {
        await _productRepository.AddRangeAsync(entities);
    }

    public void Update(Product entity)
    {
        _productRepository.Update(entity);
    }

    public void Remove(Product entity)
    {
        _productRepository.Remove(entity);
    }

    public void RemoveRange(IEnumerable<Product> entities)
    {
        _productRepository.RemoveRange(entities);
    }

    public async Task<List<Product>> GetProductsWithCategoryAsync()
    {
        string cachekey = $"ProductsWithCategory";
        var db = _redisConnection.GetDatabase(0);
        var productJson = await db.StringGetAsync(cachekey);

        if (productJson.IsNullOrEmpty)
        {
            var products = await _productRepository.GetProductsWithCategoryAsync();
            productJson = JsonConvert.SerializeObject(products,settings);
            await db.StringSetAsync(cachekey, productJson, TimeSpan.FromMinutes(3));
        }

        return JsonConvert.DeserializeObject<List<Product>>(productJson);    
    }
}