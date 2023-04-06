using System.Linq.Expressions;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWork;

namespace NLayer.Service.Services;

public class Service<T> :IService<T> where T: class
{
    private readonly IGenericRepository<T> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public Service(IGenericRepository<T> repository,IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    public async Task<T> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
        
    }

    public IEnumerable<T> GetAll()
    {
        return _repository.GetAll();
    }

    public IEnumerable<T> Where(Expression<Func<T, bool>> expression)
    {
        return _repository.Where(expression).ToList();
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
    {
        return await _repository.AnyAsync(expression);
    }

    public async Task AddAsync(T entity)
    {
        await  _repository.AddAsync(entity);
        await  _unitOfWork.CommitAsync();
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await  _repository.AddRangeAsync(entities);
        await _unitOfWork.CommitAsync();

    }

    public async Task UpdateAsync(T entity)
    {
        _repository.Update(entity);
        await _unitOfWork.CommitAsync();
    }

    public async Task RemoveAsync(T entity)
    {
        _repository.Remove(entity);
        await _unitOfWork.CommitAsync();
    }

    public async Task RemoveRangeAsync(IEnumerable<T> entities)
    {
        _repository.RemoveRange(entities);
        await  _unitOfWork.CommitAsync();

    }
}