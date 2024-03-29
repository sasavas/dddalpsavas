using CustomSolutionName.Application.Exceptions;
using CustomSolutionName.Application.Ports.Driven.DataAccess;
using CustomSolutionName.Application.Ports.Driven.DataAccess.Repositories;
using CustomSolutionName.Application.Ports.Driven.MessageBroker;
using CustomSolutionName.Domain.Features.Authentication.ValueObjects;
using CustomSolutionName.SharedLibrary.AzureServiceBus.EmailQueue;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CustomSolutionName.Application.UseCases.Users.Commands;

public record ResetPasswordCommand(string EmailAddress) : IRequest;

public class ResetPasswordRequestHandler(
    ILogger<ResetPasswordRequestHandler> logger,
    IUnitOfWork unitOfWork,
    IUserRepository userRepository,
    IMessageSenderGateway messageSenderGateway)
    : IRequestHandler<ResetPasswordCommand>
{
    public async Task Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
    {
        // check if email is a valid email address

        var foundUser = userRepository.GetByEmail(command.EmailAddress)
                        ?? throw new NotFoundException();

        unitOfWork.BeginTransaction();

        try
        {
            var code = Guid.NewGuid();

            foundUser.PasswordResetValues.Add(new PasswordResetValues(command.EmailAddress, code));
            userRepository.Update(foundUser);

            // send email for password reset
            await messageSenderGateway.SendMessageAsync(
                Constants.EmailQueueName,
                new EmailQueueMessageBody(
                    command.EmailAddress,
                    "Password Reset Request",
                    $@"
                        <h1>Your Password Reset Request</h1>
                        <p>Please follow the link to reset your password</p>
                        <a href=""http://localhost:4200/lobby/resetPassword/{code}"">Click to Reset Your Password</a>
                        <p>If you did not send a request for a password reset please ignore this email.</p> 
                        ")); //TODO: add front-end host address to configuration

            unitOfWork.Commit();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Could not start password reset request process for email: {email}", command.EmailAddress);
            unitOfWork.Rollback();
        }
    }
}