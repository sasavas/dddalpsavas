using CustomSolutionName.Application.Ports.Driven.DataAccess.Repositories;
using CustomSolutionName.Domain.Features.Authentication.RoleModule;
using MediatR;

namespace CustomSolutionName.Application.UseCases.Users.Queries;

public sealed record UserRoleRequest(Guid UserId) : IRequest<Role>;

public sealed class UserRoleRequestHandler(IUserRepository userRepository) 
    : IRequestHandler<UserRoleRequest, Role>
{
    public Task<Role> Handle(UserRoleRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(userRepository.GetUserRole(request.UserId));
    }
}