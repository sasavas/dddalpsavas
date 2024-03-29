namespace CustomSolutionName.Domain.DomainEvents;

public class UserCreatedEvent : DomainEvent
{
    public Guid UserId { get; private set; }
    public string Email { get; private set; }

    public UserCreatedEvent(Guid userId, string email)
    {
        UserId = userId;
        Email = email;
    }
}