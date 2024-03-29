using CustomSolutionName.Application.Ports.Driven.DataAccess.Contracts;
using CustomSolutionName.Domain.Features.Authentication;
using CustomSolutionName.Domain.Features.Authentication.RoleModule;

namespace CustomSolutionName.Application.Ports.Driven.DataAccess.Repositories;

public interface IUserRepository : IRepository<User, Guid>
{
    User? GetByEmail(string email);

    User? GetByEmailAndPassword(string email, string password);
    
    IEnumerable<Permission>? GetUserPermissions(Guid userId);

    Role GetUserRole(Guid userId);

    User? GetByVerificationCode(string guid);
    
    User? FindUserWithPasswordRequestCode(Guid code);
}