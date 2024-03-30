using CustomSolutionName.SharedLibrary.Exceptions;

namespace CustomSolutionName.Domain.Exceptions;

public class ValidationException : BaseException
{
    public ValidationException(ErrorCode error) : base(error)
    {
    }
}