using CustomSolutionName.Application.Ports.Driven.DataAccess.Repositories;
using CustomSolutionName.Domain.Features.Authentication.RoleModule;
using CustomSolutionName.Infrastructure.DataAccess.Repositories;

namespace CustomSolutionName.Infrastructure.DataAccess.RepositoryAdaptors;

internal class RoleRepository : SimpleRepositoryDecorator<Role, int>, IRoleRepository
{
    public RoleRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}