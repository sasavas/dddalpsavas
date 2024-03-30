using CustomSolutionName.Application.Exceptions;
using CustomSolutionName.SharedLibrary.Exceptions;

namespace CustomSolutionName.Application.UseCases.Users.Exceptions;

public class UserNotVerifiedException : AppException
{
    public UserNotVerifiedException() : base(ErrorCodes.USER_NOT_VERIFIED)
    {
    }
}