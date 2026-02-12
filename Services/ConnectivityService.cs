using Pulse_MAUI.Helpers.CustomExceptions;
using Pulse_MAUI.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pulse_MAUI.Services
{
    public class ConnectivityService : IConnectivityService
    {
        public Task CheckConnected()
        {
            if (IsInternetAvailable())
            {
                return Task.CompletedTask;
            }

            throw new NotConnectedException("No Internet connectivity available");
        }

        private static bool IsInternetAvailable()
        {
            return Connectivity.NetworkAccess == NetworkAccess.Internet;
        }

    }
}
