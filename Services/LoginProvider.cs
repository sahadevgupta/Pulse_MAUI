using Microsoft.Datasync.Client;
using Newtonsoft.Json.Linq;
using PCATablet.Core.Data;
using Pulse_MAUI.Constants;
using Pulse_MAUI.Helpers;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models;
using Pulse_MAUI.Models.Request;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Pulse_MAUI.Services
{
    public class LoginProvider(IAuthService authService) : ILoginProvider
    {
        public async Task<MobileServiceUser> LoginAsync(DatasyncClient client, IDataManager dataManager,string azureMobileServiceUrl)
        {
            MobileServiceUser user = new();
            try
            {
                var authResult = await authService.Auth(azureMobileServiceUrl);

                if (authResult is object)
                {
                    user.AuthenticationToken = authResult.ZumoAuthToken;
                    user.UserId = authResult?.ZumoUserId;
                    user.UserName = authResult?.UserId;

                    AppHelpers.IsLoggedIn = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("MobileServiceUser Login issue : " + ex.StackTrace);

            }
            return user;
        }

        public Task LogoutAsync(DatasyncClient client)
        {
            throw new NotImplementedException();
        }
    }
}
