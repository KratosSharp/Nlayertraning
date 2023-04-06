using Microsoft.EntityFrameworkCore;
using NLayer.Core;
using NLayer.Core.DTOs;
using NLayer.Core.Repositories;

namespace NLayer.Repository.Repositories;

public class ProductRepository:GenericRepository<Product>,IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<List<Product>> GetProductsWithCategoryAsync()
    {
         var products= await _context.Products.AsNoTracking().Include(x => x.Category).ToListAsync();
         return products;
    }
}