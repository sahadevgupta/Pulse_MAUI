using Microsoft.Datasync.Client;
using Pulse_MAUI.Helpers;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models;
using System.Diagnostics;

namespace Pulse_MAUI.Services
{
    public class LoginProvider(IAuthService authService) : ILoginProvider
    {
        public async Task<MobileServiceUser> LoginAsync(string azureMobileServiceUrl)
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
