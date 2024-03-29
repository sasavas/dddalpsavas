using CustomSolutionName.Application.Ports.Driven.DataAccess.Repositories;
using CustomSolutionName.Domain.Features.Authentication;
using CustomSolutionName.Domain.Features.Authentication.RoleModule;
using CustomSolutionName.Domain.Features.Authentication.ValueObjects;
using CustomSolutionName.Infrastructure.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CustomSolutionName.Infrastructure.DataAccess.RepositoryAdaptors;

internal class UserRepository : SimpleRepositoryDecorator<User, Guid>, IUserRepository
{
    public UserRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
    
    public User? GetByEmail(string email)
    {
        return BaseRepositoryImpl.GetList()
            .Include(user => user.Role) // need this?
            .SingleOrDefault(u => u.Email == new Email(email));
    }

    public User? GetByEmailAndPassword(string email, string password)
    {
        return BaseRepositoryImpl.GetList()
            .SingleOrDefault(u => u.Email == new Email(email) && u.Password == new Password(password));
    }

    public IEnumerable<Permission>? GetUserPermissions(Guid userId)
    {
        var user = BaseRepositoryImpl.GetList()
            .Include(u => u.Role.Permissions)
            .FirstOrDefault(u => u.Id == userId);
        var permissions = user?.Role?.Permissions;
        return permissions;
    }

    public Role GetUserRole(Guid userId)
    {
        var user = BaseRepositoryImpl.GetList()
            .Include(u => u.Role)
            .FirstOrDefault(u => u.Id == userId);
        return user!.Role;
    }

    public User? GetByVerificationCode(string guid)
    {
        return BaseRepositoryImpl.GetList().FirstOrDefault(user => user.VerificationCode == Guid.Parse(guid));
    }

    public User? FindUserWithPasswordRequestCode(Guid code)
    {
        return BaseRepositoryImpl
            .GetList()
            .FirstOrDefault(user => user.PasswordResetValues.Any(value => value.Code.Equals(code)));
    }
}