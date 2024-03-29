using CustomSolutionName.Domain.ErrorCodes;
using CustomSolutionName.Domain.Exceptions;

namespace CustomSolutionName.Application.Exceptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException() : base(ErrorCodes.NOT_FOUND) { }
    }
}