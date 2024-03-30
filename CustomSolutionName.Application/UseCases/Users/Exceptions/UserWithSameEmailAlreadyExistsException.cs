using CustomSolutionName.Application.Exceptions;
using CustomSolutionName.SharedLibrary.Exceptions;

namespace CustomSolutionName.Application.UseCases.Users.Exceptions;

public class UserWithSameEmailAlreadyExistsException : AppException
{
    public UserWithSameEmailAlreadyExistsException() : base(ErrorCodes.USER_EMAIL_TAKEN)
    {
    }
}