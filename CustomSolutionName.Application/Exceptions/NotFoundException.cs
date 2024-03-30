using CustomSolutionName.SharedLibrary.Exceptions;

namespace CustomSolutionName.Application.Exceptions
{
    public class NotFoundException : AppException
    {
        public NotFoundException() : base(ErrorCodes.NOT_FOUND) { }
    }
}