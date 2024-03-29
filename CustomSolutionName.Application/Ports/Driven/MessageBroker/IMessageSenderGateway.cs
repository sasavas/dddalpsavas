namespace CustomSolutionName.Application.Ports.Driven.MessageBroker;

/// <summary>
/// Service to send messages to a message broker service such as Azure Service Bus or RabbitMQ (depends on the implementation)
/// </summary>
public interface IMessageSenderGateway
{
    public Task SendMessageAsync(string queueName, object messageContent);
}