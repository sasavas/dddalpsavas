using CustomSolutionName.Domain.ErrorCodes;
using CustomSolutionName.Domain.Exceptions;

namespace CustomSolutionName.Application.UseCases.Users.Exceptions;

public class UserNotVerifiedException : BaseException
{
    public UserNotVerifiedException() : base(ErrorCodes.USER_NOT_VERIFIED)
    {
    }
}