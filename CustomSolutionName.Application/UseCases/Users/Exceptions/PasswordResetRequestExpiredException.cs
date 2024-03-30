using CustomSolutionName.Application.Exceptions;
using CustomSolutionName.SharedLibrary.Exceptions;

namespace CustomSolutionName.Application.UseCases.Users.Exceptions;

public class PasswordResetRequestExpiredException : AppException
{
    public PasswordResetRequestExpiredException() 
        : base(ErrorCodes.USER_PASSWORD_RESET_REQUEST_EXPIRED)
    {
    }
}