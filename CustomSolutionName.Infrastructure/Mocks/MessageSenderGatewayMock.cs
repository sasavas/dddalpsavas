using CustomSolutionName.Application.Ports.Driven.MessageBroker;

namespace CustomSolutionName.Infrastructure.Mocks;

public class MessageSenderGatewayMock : IMessageSenderGateway
{
    public Task SendMessageAsync(string queueName, object messageContent)
    {
        Console.WriteLine($"Message sent to queue {queueName}, message content: {messageContent}");
        return Task.CompletedTask;
    }
}