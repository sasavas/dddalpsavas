using CustomSolutionName.Domain.ErrorCodes;
using CustomSolutionName.Domain.Exceptions;

namespace CustomSolutionName.Application.Exceptions;

public class LangfellaApplicationException : BaseException
{
    public LangfellaApplicationException(ErrorCode errorCode) : base(errorCode)
    {
    }
}