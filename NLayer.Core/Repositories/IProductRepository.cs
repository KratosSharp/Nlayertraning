using System.Globalization;
using NLayer.Core.DTOs;

namespace NLayer.Core.Repositories;

public interface IProductRepository:IGenericRepository<Product>
{
    Task<List<Product>> GetProductsWithCategoryAsync();
}