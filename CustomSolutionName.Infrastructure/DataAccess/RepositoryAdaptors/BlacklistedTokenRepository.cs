using CustomSolutionName.Application.Ports.Driven.DataAccess.Repositories;
using CustomSolutionName.Domain.Features.Authentication;
using CustomSolutionName.Infrastructure.DataAccess.Repositories;

namespace CustomSolutionName.Infrastructure.DataAccess.RepositoryAdaptors;

internal class BlacklistedTokenRepository : SimpleRepositoryDecorator<BlacklistedToken, long>, IBlacklistedTokenRepository
{
    public BlacklistedTokenRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
    
    public bool IsTokenBlacklisted(string token)
    {
        var result = GetList().SingleOrDefault(bt => bt.Token == token);
        return result is not null;
    }
}