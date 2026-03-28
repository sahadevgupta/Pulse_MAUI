using System;
using System.Runtime.Serialization;

namespace Pulse_MAUI.Helpers.CustomExceptions;

public class AuthServiceNoCachedInfoException : Exception
{
    public AuthServiceNoCachedInfoException()
    {

    }

    public AuthServiceNoCachedInfoException(string message) : base(message)
    {

    }

    public AuthServiceNoCachedInfoException(string message, Exception innerException) : base(message, innerException)
    {

    }

    protected AuthServiceNoCachedInfoException(SerializationInfo info, StreamingContext context) : base(info, context)
    {

    }
}
