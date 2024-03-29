using CustomSolutionName.Application.Exceptions;
using CustomSolutionName.Application.Ports.Driven.DataAccess.Repositories;
using CustomSolutionName.Domain.Features.Authentication;
using MediatR;

namespace CustomSolutionName.Application.UseCases.Users.Queries;

public record LoginRequest(string Email, string Password)
    : IRequest<User>;

public sealed class LoginRequestHandler
    : IRequestHandler<LoginRequest, User>
{
    private readonly IUserRepository _userRepository;

    public LoginRequestHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<User> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = _userRepository.GetByEmailAndPassword(
            request.Email, 
            request.Password);
        
        if (user is null)
            throw new NotFoundException();

        //TODO:production
        // if (user.IsVerified == false)
        //     throw new UserNotVerifiedException();

        return Task.FromResult(user);
    }
}