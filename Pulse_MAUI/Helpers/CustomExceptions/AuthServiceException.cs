using System;
using System.Runtime.Serialization;

namespace Pulse_MAUI.Helpers.CustomExceptions;

[Serializable]
public class AuthServiceException : Exception
{
    public AuthServiceException()
    {

    }

    public AuthServiceException(string message) : base(message)
    {

    }

    public AuthServiceException(string message, Exception innerException) : base(message, innerException)
    {

    }

    protected AuthServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
    {

    }
}
