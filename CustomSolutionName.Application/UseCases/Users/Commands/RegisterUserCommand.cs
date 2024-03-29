using CustomSolutionName.Application.Ports.Driven.DataAccess;
using CustomSolutionName.Application.Ports.Driven.DataAccess.Repositories;
using CustomSolutionName.Application.Ports.Driven.MessageBroker;
using CustomSolutionName.Application.UseCases.Users.Exceptions;
using CustomSolutionName.Domain.Features.Authentication;
using CustomSolutionName.Domain.Features.Authentication.ValueObjects;
using CustomSolutionName.SharedLibrary.AzureServiceBus.EmailQueue;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CustomSolutionName.Application.UseCases.Users.Commands;

public record RegisterUserCommand(
    string Email,
    string Password,
    string LanguageCode) : IRequest<User>;

public class RegisterUserCommandHandler(
    IUnitOfWork unitOfWork,
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    IMessageSenderGateway messageSenderGateway,
    ILogger<RegisterUserCommandHandler> logger) : IRequestHandler<RegisterUserCommand, User>
{
    public async Task<User> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = userRepository.GetByEmail(request.Email);
        if (existingUser is not null)
        {
            throw new UserWithSameEmailAlreadyExistsException();
        }

        var defaultUserRole = roleRepository.GetList().Single(role => role.Name.Equals("User"));
        
        try
        {
            unitOfWork.BeginTransaction();

            // create and save the user
            var verificationCode = Guid.NewGuid();
            
            var user = User.Create(new Email(request.Email),
                                   new Password(request.Password),
                                   defaultUserRole,
                                   verificationCode);
            
            var createdUser = userRepository.Create(user);
            
            // Send email to user-to-register
            await messageSenderGateway.SendMessageAsync(
                Constants.EmailQueueName,
                new EmailQueueMessageBody(request.Email,
                    "Welcome to MyApp",
                    $"""
                         <h1>Welcome to MyApp!</h1>
                         <p>Please verify to complete your registration process</p>
                         <a href="http://localhost:4200/lobby/verification/{verificationCode}">Click to Verify</a>
                     """));

            unitOfWork.Commit();

            return createdUser;
        }
        catch (Exception e)
        {
            unitOfWork.Rollback();
            logger.Log(LogLevel.Error, e, "Error occurred while user registration");
            throw;
        }
    }
}