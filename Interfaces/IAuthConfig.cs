using System;
using System.Collections.Generic;
using System.Text;

namespace Pulse_MAUI.Interfaces
{
    public interface IAuthConfig
    {
        public string ClientId { get; }

        public string TenantId { get; }

        public string Authority { get; }

        public string RedirectUri { get; }

        public string[] GraphApiScopes { get; }

        public string[] Scopes { get; }

    }
}
