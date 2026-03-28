using System;
using System.Runtime.Serialization;

namespace Pulse_MAUI.Helpers.CustomExceptions;

[Serializable]
public class AuthServiceUserCanceledException : AuthServiceInteractiveLogonRequiredException
{
    
    public AuthServiceUserCanceledException(string message, Exception innerException) : base(message, innerException)
    {

    }

}
