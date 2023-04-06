using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NLayer.Core.Repositories;

namespace NLayer.Repository.Repositories;

public class GenericRepository<T>:IGenericRepository<T> where T:class
{
    protected readonly AppDbContext _context;
    private readonly DbSet<T> dbSet;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
        dbSet = context.Set<T>();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await dbSet.FindAsync(id);
    }

    public IQueryable<T> GetAll()
    {
        return dbSet.AsNoTracking().AsQueryable();
    }

    public IQueryable<T> Where(Expression<Func<T, bool>> expression)
    {
        return dbSet.Where(expression);
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
    {
        return await dbSet.AnyAsync(expression);
    }

    public async Task AddAsync(T entity)
    {
        await  dbSet.AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await dbSet.AddRangeAsync(entities);
    }

    public void Update(T entity)
    {
        dbSet.Update(entity);
    }

    public void Remove(T entity)
    {
        dbSet.Remove(entity);
    }
    
    public void RemoveRange(IEnumerable<T> entities)
    {
        dbSet.RemoveRange(entities);
    }
}