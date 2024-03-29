using CustomSolutionName.Domain.ErrorCodes;

namespace CustomSolutionName.Domain.Exceptions;

public abstract class BaseException : Exception
{
    protected string CODE;

    protected BaseException(ErrorCode errorCode) : base(errorCode.DESCRIPTION)
    {
        CODE = errorCode.CODE;
    }
}