using System;
using System.Runtime.Serialization;

namespace Pulse_MAUI.Helpers.CustomExceptions;

[Serializable]
public class AuthDriverUIRequiredException : Exception
{
    public AuthDriverUIRequiredException()
    {

    }

    public AuthDriverUIRequiredException(string message) : base(message)
    {

    }

    public AuthDriverUIRequiredException(string message, Exception innerException) : base(message, innerException)
    {

    }

    protected AuthDriverUIRequiredException(SerializationInfo info, StreamingContext context) : base(info, context)
    {

    }
}
