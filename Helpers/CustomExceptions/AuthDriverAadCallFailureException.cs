using System;

namespace Pulse_MAUI.Helpers.CustomExceptions;

[Serializable]
public class AuthDriverAadCallFailureException : AuthServiceException
{
    public AuthDriverAadCallFailureException(string message, Exception innerException) : base(message, innerException)
    {

    }
}
