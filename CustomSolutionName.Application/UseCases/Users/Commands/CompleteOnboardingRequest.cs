using CustomSolutionName.Application.Exceptions;
using CustomSolutionName.Application.Ports.Driven.DataAccess;
using CustomSolutionName.Application.Ports.Driven.DataAccess.Repositories;
using CustomSolutionName.Domain.Features.Authentication.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CustomSolutionName.Application.UseCases.Users.Commands;

public record CompleteOnboardingRequest(
    string? FirstName,
    string? LastName,
    string? Gender,
    DateOnly? DateOfBirth) : IRequest;

public class CompleteOnboardingRequestHandler(
    IUserIdProvider userIdProvider,
    IUnitOfWork unitOfWork,
    IUserRepository userRepository,
    ILogger<CompleteOnboardingRequestHandler> logger)
    : IRequestHandler<CompleteOnboardingRequest>
{
    public Task Handle(CompleteOnboardingRequest request, CancellationToken cancellationToken)
    {
        var foundUser = userRepository.GetById(userIdProvider.GetUserId())
                        ?? throw new NotFoundException();

        if (request.FirstName != null) foundUser.FirstName = request.FirstName;
        if (request.LastName != null) foundUser.LastName = request.LastName;
        if (request.DateOfBirth != null) foundUser.DateOfBirth = request.DateOfBirth;
        if (request.Gender != null) foundUser.Gender = new Gender(request.Gender);
        
        unitOfWork.BeginTransaction();
        try
        {
            userRepository.Update(foundUser);
            unitOfWork.Commit();
        }
        catch (Exception e)
        {
            unitOfWork.Rollback();
            logger.LogError(e, "Error while saving User Onboarding Info");
        }

        return Task.CompletedTask;
    }
}