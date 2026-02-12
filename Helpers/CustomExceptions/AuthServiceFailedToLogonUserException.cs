using System;

namespace Pulse_MAUI.Helpers.CustomExceptions;

[Serializable]
public class AuthServiceFailedToLogonUserException : AuthServiceException
{
    public AuthServiceFailedToLogonUserException(string message, Exception innerException) : base(message, innerException)
    {

    }
}
