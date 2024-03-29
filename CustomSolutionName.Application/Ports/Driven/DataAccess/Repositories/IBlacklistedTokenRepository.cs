using CustomSolutionName.Application.Ports.Driven.DataAccess.Contracts;
using CustomSolutionName.Domain.Features.Authentication;

namespace CustomSolutionName.Application.Ports.Driven.DataAccess.Repositories;

public interface IBlacklistedTokenRepository : IRepository<BlacklistedToken, long>
{
    bool IsTokenBlacklisted(string token);
}