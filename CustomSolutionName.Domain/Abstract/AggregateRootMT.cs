namespace CustomSolutionName.Domain.Abstract;

/// <summary>
/// Multi-language Aggregate Root
/// </summary>
/// <typeparam name="TId">Data type of the Id field</typeparam>
public abstract class AggregateRootMt<TId> : AggregateRoot<TId> 
    where TId: notnull
{
    public Guid LearnerId { get; set; }
    
    protected AggregateRootMt(TId id) : base(id)
    {
    }

    protected AggregateRootMt()
    {
    }
}