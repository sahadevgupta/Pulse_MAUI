using System;

namespace Pulse_MAUI.Helpers.CustomExceptions;

[Serializable]
public class AuthServiceNotConnectedException : AuthServiceException
{
    public AuthServiceNotConnectedException(string message, Exception innerException) : base(message, innerException)
    {

    }
}
