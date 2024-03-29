using CustomSolutionName.Domain.ErrorCodes;
using CustomSolutionName.Domain.Exceptions;

namespace CustomSolutionName.Application.UseCases.Users.Exceptions;

public class PasswordResetRequestExpiredException : BaseException
{
    public PasswordResetRequestExpiredException() 
        : base(ErrorCodes.USER_PASSWORD_RESET_REQUEST_EXPIRED)
    {
    }
}