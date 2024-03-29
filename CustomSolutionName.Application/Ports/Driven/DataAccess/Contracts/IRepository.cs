using System.Linq.Expressions;
using CustomSolutionName.Domain.Abstract;

namespace CustomSolutionName.Application.Ports.Driven.DataAccess.Contracts;

public interface IRepository<TAggregateRoot, in TId> 
    where TAggregateRoot : AggregateRoot<TId>
    where TId : notnull
{
    TAggregateRoot? GetById(TId id);

    IEnumerable<TAggregateRoot> GetList();
    
    IEnumerable<TAggregateRoot> GetList(Expression<Func<TAggregateRoot, bool>> filter);

    TAggregateRoot Create(TAggregateRoot entity);

    TAggregateRoot Update(TAggregateRoot entity);

    void Delete(TId entityId);
}