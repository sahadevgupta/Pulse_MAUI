using System;
using System.Collections.Generic;
using System.Text;

namespace Pulse_MAUI.Helpers.CustomExceptions
{
    [System.Serializable]
    public class UnAuthorizedException : System.Exception
    {
        public UnAuthorizedException() { }
        public UnAuthorizedException(string message) : base(message) { }
        public UnAuthorizedException(string message, System.Exception inner) : base(message, inner) { }
        protected UnAuthorizedException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
