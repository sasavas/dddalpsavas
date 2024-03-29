using System.Linq.Expressions;
using CustomSolutionName.Application.Ports.Driven.DataAccess;
using CustomSolutionName.Application.Ports.Driven.DataAccess.Contracts;
using CustomSolutionName.Domain.Abstract;

namespace CustomSolutionName.Infrastructure.DataAccess.Repositories;

public class MtRepositoryDecorator<TEntity, TId> : IMtRepository<TEntity, TId> 
    where TEntity : AggregateRootMt<TId> where TId : notnull
{
    protected readonly BaseRepositoryImpl<TEntity, TId> BaseRepositoryImpl;
    protected readonly Guid _learnerId;
    
    internal MtRepositoryDecorator(
        IUserIdProvider userIdProvider,
        AppDbContext dbContext)
    {
        _learnerId = userIdProvider.GetUserId();
        BaseRepositoryImpl = new BaseRepositoryImpl<TEntity, TId>(dbContext);
    }
    
    public TEntity Create(TEntity entity)
    {
        entity.LearnerId = _learnerId;
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

    public virtual TEntity? GetById(TId id)
    {
        return BaseRepositoryImpl.GetById(id);
    }

    public virtual IEnumerable<TEntity> GetList()
    {
        return BaseRepositoryImpl.GetList()
            .Where(e => e.LearnerId == _learnerId);
    }

    public virtual IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> filter)
    {
        return BaseRepositoryImpl.GetList(filter)
            .Where(e => e.LearnerId == _learnerId);
    }
    
    public virtual TEntity CreateLibraryItem(TEntity entity)
    {
        entity.LearnerId = default; // not belonging to a specific tenant, hence the library item
        var inserted = BaseRepositoryImpl.Create(entity);
        return inserted;
    }
}