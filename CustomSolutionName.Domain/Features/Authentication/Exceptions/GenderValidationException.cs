using CustomSolutionName.Domain.Exceptions;

namespace CustomSolutionName.Domain.Features.Authentication.Exceptions;

public class GenderValidationException : ValidationException
{
    public GenderValidationException() : base(ErrorCodes.ErrorCodes.USER_NOT_VALID_GENDER)
    {
    }
}