using CustomSolutionName.Application.Exceptions;
using CustomSolutionName.Application.Ports.Driven.DataAccess;
using CustomSolutionName.Application.Ports.Driven.DataAccess.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CustomSolutionName.Application.UseCases.Users.Commands;

public record VerifyUserCommand(string verificationCode) : IRequest<bool>;

public class VerifyUserCommandHandler(
    IUnitOfWork unitOfWork, IUserRepository userRepository, ILogger<VerifyUserCommandHandler> logger) 
    : IRequestHandler<VerifyUserCommand, bool>
{
    public Task<bool> Handle(VerifyUserCommand request, CancellationToken cancellationToken)
    {
        var user = userRepository.GetByVerificationCode(request.verificationCode);
        if (user is null)
        {
            throw new NotFoundException();
        }

        try
        {
            unitOfWork.BeginTransaction();
            user.IsVerified = true;
            unitOfWork.Commit();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Could not verify user");
            throw;
        }
        
        return Task.FromResult(true);
    }
}