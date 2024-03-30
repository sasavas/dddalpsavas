namespace CustomSolutionName.SharedLibrary.Exceptions;

public abstract class BaseException : Exception
{
    protected string Code { get; }
    protected string Description { get; }

    protected BaseException(ErrorCode errorCode) : base(errorCode.DESCRIPTION)
    {
        Code = errorCode.CODE;
        Description = errorCode.DESCRIPTION;
    }
}