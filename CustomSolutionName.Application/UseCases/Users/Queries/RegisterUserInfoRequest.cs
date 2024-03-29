using CustomSolutionName.Domain.Features.Authentication.ValueObjects;
using MediatR;

namespace CustomSolutionName.Application.UseCases.Users.Queries;

public sealed record RegisterUserInfoRequest : IRequest<RegisterUserInfoDto>;

public sealed record RegisterUserInfoDto(
    IEnumerable<string> GenderCodes);


public sealed class RegisterUserInfoRequestHandler
    : IRequestHandler<RegisterUserInfoRequest, RegisterUserInfoDto>
{
    public Task<RegisterUserInfoDto> Handle(RegisterUserInfoRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(
            new RegisterUserInfoDto(
                Gender.AllGenders.Select(gender => gender.Value)));
    }
}