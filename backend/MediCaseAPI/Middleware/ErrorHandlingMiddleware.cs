using Amazon.Runtime;
using MediCaseAPI.DataTransferObjects;
using MediCaseAPI.Exceptions;

namespace MediCaseAPI.Middleware;

public class ErrorHandlingMiddleware
{
    private const string InternalErrorMessage = "Internal server error.";
    private const string ExternalServiceMessage = "An AWS service is not available. Check the AWS configuration.";

    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (AppException ex)
        {
            _logger.LogWarning("{Path}: {Message}", context.Request.Path, ex.Message);
            await WriteErrorAsync(context, ex.StatusCode, ex.Message);
        }
        catch (Exception ex) when (ex is AmazonServiceException or AmazonClientException)
        {
            _logger.LogWarning(ex, "AWS request failed for {Path}", context.Request.Path);
            await WriteErrorAsync(context, StatusCodes.Status502BadGateway, ExternalServiceMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception for {Path}", context.Request.Path);
            await WriteErrorAsync(context, StatusCodes.Status500InternalServerError, InternalErrorMessage);
        }
    }

    private static Task WriteErrorAsync(HttpContext context, int statusCode, string message)
    {
        context.Response.StatusCode = statusCode;
        return context.Response.WriteAsJsonAsync(new ErrorDetailsDTO(statusCode, message));
    }
}
