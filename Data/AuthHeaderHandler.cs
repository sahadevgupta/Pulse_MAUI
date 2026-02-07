using System;
using System.Collections.Generic;
using System.Text;

namespace Pulse_MAUI.Data
{
    public class AuthHeaderHandler : DelegatingHandler
    {
        /// <summary>
        /// Overrides the send async method. Currently not used.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="request">Request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return await base.SendAsync(request, cancellationToken);
        }

    }
}
