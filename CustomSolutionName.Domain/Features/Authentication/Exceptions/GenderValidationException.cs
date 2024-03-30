using CustomSolutionName.Domain.Exceptions;
using CustomSolutionName.SharedLibrary.Exceptions;

namespace CustomSolutionName.Domain.Features.Authentication.Exceptions;

public class GenderValidationException : ValidationException
{
    public GenderValidationException() : base(ErrorCodes.USER_NOT_VALID_GENDER)
    {
    }
}