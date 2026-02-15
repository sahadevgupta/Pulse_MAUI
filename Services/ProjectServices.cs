using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pulse_MAUI.Services
{
    public class ProjectServices(IAuthService authService,
        IProjectBackendService projectBackendService) : ApiServiceBase(authService), IProjectServices 
    { 

        public async Task<string> GetAppConfigAsync()
        {
            string result = string.Empty;
            try
            {
                var headers = await GetHeader();
                result = await projectBackendService.GetAppConfigAsync(headers).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var error = ex.Message;
            }
            return result;
        }

        public async Task<string> GetAzureConnectionAsync()
        {
            string result = string.Empty;
            try
            {
                var headers = await GetHeader();
                result = await projectBackendService.GetAzureConnectionAsync(headers).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var error = ex.Message;
            }
            return result;
        }
        public async Task PostSyncLogAsync(SyncLogRequest syncLogRequest)
        {
            try
            {
                var headers = await GetHeader();
                await projectBackendService.PostSyncLogAsync(headers, syncLogRequest).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var error = ex.Message;
            }
        }

    }
}
