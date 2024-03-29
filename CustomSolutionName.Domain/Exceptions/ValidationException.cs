using CustomSolutionName.Domain.ErrorCodes;

namespace CustomSolutionName.Domain.Exceptions;

public class ValidationException : BaseException
{
    public ValidationException(ErrorCode errorCode) : base(errorCode)
    {
    }
}