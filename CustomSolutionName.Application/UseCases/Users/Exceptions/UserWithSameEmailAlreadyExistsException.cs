using CustomSolutionName.Domain.ErrorCodes;
using CustomSolutionName.Domain.Exceptions;

namespace CustomSolutionName.Application.UseCases.Users.Exceptions;

public class UserWithSameEmailAlreadyExistsException : BaseException
{
    public UserWithSameEmailAlreadyExistsException() : base(ErrorCodes.USER_EMAIL_TAKEN)
    {
    }
}