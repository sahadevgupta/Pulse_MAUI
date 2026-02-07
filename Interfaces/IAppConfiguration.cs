using System;
using System.Collections.Generic;
using System.Text;

namespace Pulse_MAUI.Interfaces
{
    public interface IAppConfiguration : IAuthConfig
    {
        string BaseUrl { get; }
    }
}
