using CustomSolutionName.Application.Ports.Driven.DataAccess.Contracts;
using CustomSolutionName.Domain.Features.Authentication.RoleModule;

namespace CustomSolutionName.Application.Ports.Driven.DataAccess.Repositories;

public interface IRoleRepository : IRepository<Role, int>
{
    
}