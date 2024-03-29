using System.Diagnostics;
using CustomSolutionName.Api.Extensions;
using CustomSolutionName.Domain.ErrorCodes;
using CustomSolutionName.Domain.Exceptions;
using EntityFramework.Exceptions.Common;
using Microsoft.EntityFrameworkCore;

namespace CustomSolutionName.Api.MiddleWares;

public class ExceptionMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IWebHostEnvironment _environment;

    public ExceptionMiddleware(
        ILogger<ExceptionMiddleware> logger, IWebHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (BaseException e)
        {
            _logger.LogInformation(e, "CustomSolutionName.Domain exception occurred with message");

            await Results.Problem(
                    detail: e.Message,
                    statusCode: StatusCodes.Status400BadRequest,
                    type: ErrorCodes.APP_ERROR.CODE)
                .ExecuteAsync(context);
        }
        catch (UniqueConstraintException e)
        {
            _logger.LogInformation(e, "There is already an entry in the database with the same key");
            
            await Results.Problem(
                    detail: "Duplicate key exception",
                    statusCode: StatusCodes.Status409Conflict,
                    type: ErrorCodes.DUPLICATE_ENTITY.CODE)
                .ExecuteAsync(context);
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, "Could not finish database operation successfully");
            
            await Results.Problem(
                    detail: "Could not finish database operation successfully",
                    statusCode: StatusCodes.Status500InternalServerError,
                    type: ErrorCodes.DATABASE_UPDATE_ERROR.CODE)
                .ExecuteAsync(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not process a request with exception");
            
            await Results.Problem(
                    _environment.IsDevelopment() ? e.WithStackTrace() : "Oops, we made a mistake",
                    "",
                    StatusCodes.Status500InternalServerError,
                    "An Error occurred",
                    ErrorCodes.INTERNAL_ERROR.CODE,
                    new Dictionary<string, object?>()
                    {
                        {"traceId", Activity.Current?.Id}
                    })
                .ExecuteAsync(context);
        }
    }
}