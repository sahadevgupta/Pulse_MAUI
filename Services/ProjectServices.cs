using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models;
using Pulse_MAUI.Models.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pulse_MAUI.Services
{
    public class ProjectServices(IAuthService authService,
        IProjectBackendService projectBackendService,
        ILookupService lookupService) : ApiServiceBase(authService), IProjectServices
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

        /// <summary>
		/// Works out the default project for a given engineer.
		/// </summary>
		/// <returns>The default project.</returns>
		public async Task<Project?> GetDefaultProject()
        {
            var projects = await lookupService.GetProjectListAsync();

            if (projects != null && projects.Count() > 0)
                return projects.ToList()[0];
            else
                return null;
        }

    }
}
