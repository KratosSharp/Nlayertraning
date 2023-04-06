namespace NLayer.Core.Repositories;

public interface ICategoryRepository:IGenericRepository<Category>
{
    Task<Category> GetSinglecategoryWithProducts(int categoryId);
}