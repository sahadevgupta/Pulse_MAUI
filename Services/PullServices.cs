using Pulse_MAUI.Helpers;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pulse_MAUI.Services
{
    public class PullServices(IAuthService authService, ISyncServiceApi syncServiceApi) : ApiServiceBase(authService), IPullServices
    {
        public async Task GetActivitiesFromServerToDBAsync()
        {
            try
            {
                var headers = await GetHeader();
                var activities = await syncServiceApi.GetActivitiesAsync(headers).ConfigureAwait(false);

                var activityRepo = AppHelpers.GetRepository<Activity>();
                await activityRepo.DeleteAllAsync();
                await activityRepo.InsertAllAsync(activities);

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
