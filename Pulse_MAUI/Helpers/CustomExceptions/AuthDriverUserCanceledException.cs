using System;

namespace Pulse_MAUI.Helpers.CustomExceptions;

[Serializable]
public class AuthDriverUserCanceledException : AuthServiceException
{
    public AuthDriverUserCanceledException(string message, Exception innerException) : base(message, innerException)
    {

    }
}
