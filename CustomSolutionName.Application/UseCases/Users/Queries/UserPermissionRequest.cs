using CustomSolutionName.Application.Ports.Driven.DataAccess.Repositories;
using CustomSolutionName.Domain.Features.Authentication.RoleModule;
using MediatR;

namespace CustomSolutionName.Application.UseCases.Users.Queries;

public record UserPermissionRequest(Guid UserId) : IRequest<IEnumerable<Permission>?>;

public class UserPermissionRequestHandler : IRequestHandler<UserPermissionRequest, IEnumerable<Permission>?>
{
    private readonly IUserRepository _userRepository;

    public UserPermissionRequestHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<IEnumerable<Permission>?> Handle(UserPermissionRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_userRepository.GetUserPermissions(request.UserId));
    }
}