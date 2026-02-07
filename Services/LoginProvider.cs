using Microsoft.Datasync.Client;
using Newtonsoft.Json.Linq;
using PCATablet.Core.Data;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Pulse_MAUI.Services
{
    public class LoginProvider(IAuthDriver authDriver, IProjectBackendService backendAPI) : ILoginProvider
    {
        public async Task<object> LoginAsync(DatasyncClient client, DataManager dataManager)
        {
            try
            {

                var ar = await authDriver.AuthenticateUser();

                if (ar is object)
                {
                    //client.CurrentUser = user;
                    var header = new Dictionary<string, string>
                    {
                        { "Authorization", ar.AccessToken! }
                    };
                    
                    var user = await backendAPI.GetUserInfoAsync(header);

                    //d.CurrentClient = new MobileServiceClient(client.MobileAppUri, new ReqHandler(ar.Token));
                    //d.CurrentClient.CurrentUser = user;

                    //user.UserId = ar.Account.Username;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("MobileServiceUser Login issue : " + ex.StackTrace);

            }
            return null;
        }

        public Task LogoutAsync(DatasyncClient client)
        {
            throw new NotImplementedException();
        }
    }
}
