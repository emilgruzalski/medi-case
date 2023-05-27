namespace MediCaseAPI.Exceptions;

public class ExternalServiceException : AppException
{
    public ExternalServiceException(string message)
        : base(message, StatusCodes.Status502BadGateway)
    {
    }
}
