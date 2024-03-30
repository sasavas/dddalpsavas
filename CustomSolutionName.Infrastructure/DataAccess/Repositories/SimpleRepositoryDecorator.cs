using System.Linq.Expressions;
using CustomSolutionName.Application.Ports.Driven.DataAccess.Contracts;
using CustomSolutionName.Domain.Abstract;

namespace CustomSolutionName.Infrastructure.DataAccess.Repositories;

public class SimpleRepositoryDecorator<TEntity, TId> : IRepository<TEntity, TId> 
    where TEntity : AggregateRoot<TId> where TId : notnull
{
    protected readonly BaseRepositoryImpl<TEntity, TId> BaseRepositoryImpl;

    internal SimpleRepositoryDecorator(AppDbContext dbContext)
    {
        BaseRepositoryImpl = new BaseRepositoryImpl<TEntity, TId>(dbContext);
    }
    
    public TEntity Insert(TEntity entity)
    {
        return BaseRepositoryImpl.Create(entity);
    }

    public TEntity Update(TEntity entity)
    {
        return BaseRepositoryImpl.Update(entity);
    }

    public void Delete(TId id)
    {
        BaseRepositoryImpl.Delete(id);
    }

    public TEntity? GetById(TId id)
    {
        return BaseRepositoryImpl.GetById(id);
    }

    public IEnumerable<TEntity> GetList()
    {
        return BaseRepositoryImpl.GetList();
    }

    public IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> filter)
    {
        return BaseRepositoryImpl.GetList(filter);
    }
}