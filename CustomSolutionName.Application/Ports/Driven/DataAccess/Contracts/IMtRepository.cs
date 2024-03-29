using CustomSolutionName.Domain.Abstract;

namespace CustomSolutionName.Application.Ports.Driven.DataAccess.Contracts;

public interface IMtRepository<TAggregateRoot, in TId> : IRepository<TAggregateRoot, TId> 
    where TAggregateRoot : AggregateRootMt<TId> where TId : notnull
{
    TAggregateRoot CreateLibraryItem(TAggregateRoot entity);
}