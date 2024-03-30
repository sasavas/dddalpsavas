using CustomSolutionName.SharedLibrary.Exceptions;

namespace CustomSolutionName.Application.Exceptions;

public class AppException : BaseException
{
    public AppException(ErrorCode errorCode) : base(errorCode)
    {
    }
}