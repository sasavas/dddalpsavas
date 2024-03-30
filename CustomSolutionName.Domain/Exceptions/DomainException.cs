using CustomSolutionName.SharedLibrary.Exceptions;

namespace CustomSolutionName.Domain.Exceptions;

public class DomainException : BaseException
{
    protected DomainException(ErrorCode error) : base(error)
    {
    }
}