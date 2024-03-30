using CustomSolutionName.Application.Exceptions;
using CustomSolutionName.Application.Ports.Driven.DataAccess;
using CustomSolutionName.Application.Ports.Driven.DataAccess.Repositories;
using CustomSolutionName.Application.UseCases.Users.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CustomSolutionName.Application.UseCases.Users.Commands;

public record VerifyPasswordResetCommand(Guid Code, string NewPassword) : IRequest;

public class VerifyPasswordResetRequestHandler(
    ILogger<VerifyPasswordResetRequestHandler> logger,
    IUnitOfWork unitOfWork,
    IUserRepository userRepository)
    : IRequestHandler<VerifyPasswordResetCommand>
{
    public Task Handle(VerifyPasswordResetCommand command, CancellationToken cancellationToken)
    {
        var foundUser =
            userRepository.GetUserWithPasswordRequestCode(command.Code)
            ?? throw new NotFoundException();

        var hasValidPasswordResetRequest = foundUser.HasValidPasswordResetRequest();
        if (!hasValidPasswordResetRequest)
        {
            throw new PasswordResetRequestExpiredException();
        } 

        unitOfWork.BeginTransaction();
        try
        {
            foundUser.UpdatePassword(command.NewPassword);
            userRepository.Update(foundUser);
            
            unitOfWork.Commit();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Could not verify password reset");
        }

        return Task.CompletedTask;
    }
}