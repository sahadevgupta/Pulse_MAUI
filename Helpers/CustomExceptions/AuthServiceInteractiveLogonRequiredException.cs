using System;
using System.Runtime.Serialization;

namespace Pulse_MAUI.Helpers.CustomExceptions;

[Serializable]
public class AuthServiceInteractiveLogonRequiredException : AuthServiceException
{
    public AuthServiceInteractiveLogonRequiredException()
    {

    }

    public AuthServiceInteractiveLogonRequiredException(string message) : base(message)
    {

    }

    public AuthServiceInteractiveLogonRequiredException(string message, Exception innerException) : base(message, innerException)
    {

    }

    protected AuthServiceInteractiveLogonRequiredException(SerializationInfo info, StreamingContext context) : base(info, context)
    {

    }
}
