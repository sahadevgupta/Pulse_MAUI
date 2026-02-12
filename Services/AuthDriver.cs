using Microsoft.Identity.Client;
using Pulse_MAUI.Constants;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models.Response;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;


#if IOS
using UIKit;
#endif

namespace Pulse_MAUI.Services
{
    public class AuthDriver : IAuthDriver
    {
        private readonly IAuthConfig _configuration;
        private HttpClient _httpClient { get; set; }
        private IPublicClientApplication PCA { get; set; }

        public AuthDriver(IAuthConfig config)
        {
            _configuration = config;
            PublicClientApplicationBuilder pca = null;

            pca = PublicClientApplicationBuilder
                .CreateWithApplicationOptions(new PublicClientApplicationOptions
                {
                    TenantId = _configuration.TenantId,
                    ClientId = _configuration.ClientId,
                    RedirectUri = _configuration.RedirectUri,
                })
                .WithAuthority(config.Authority)
#if IOS
                .WithIosKeychainSecurityGroup("com.liscr.Seafarer")
#endif
#if ANDROID
               .WithParentActivityOrWindow(() => Platform.CurrentActivity)
#endif
                           ;

            PCA = pca?.Build();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1416:This call site is reachable on: 'iOS' 14.2 and later, 'maccatalyst' 14.2 and later.", Justification = "MAUI we uses only latest platform API.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1416:This call site is reachable on: 'Android' 21.0 and later.", Justification = "MAUI we uses only latest platform API.")]
        public async Task<AuthResultDto> AuthenticateUser(string azureMobileAppsBackendUrl)
        {
            InitializeHttpClient(azureMobileAppsBackendUrl);
            var result = Preferences.Get(ADConstants.ObjectId, string.Empty);
            var authResult = await AcquireTokenAsync(result);
            var authInfo =  GetAuthResultDto(authResult);
            var response =  await ExchangeTokenForZumoAsync(authResult.IdToken, authInfo);
            if (response.Item1)
                return response.Item2;
            return null;
        }

        private void InitializeHttpClient(string azureMobileAppsBackendUrl)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(azureMobileAppsBackendUrl),
                Timeout = TimeSpan.FromSeconds(30)
            };

            // Set default headers similar to MobileServiceClient
            _httpClient.DefaultRequestHeaders.Add("ZUMO-API-VERSION", "2.0.0");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Exchanges an Azure AD access token for an Azure Mobile Apps ZUMO authentication token.
        /// This replicates the behavior of IMobileServiceClient.LoginAsync().
        /// </summary>
        /// <param name="azureAdToken">The Azure AD access token to exchange.</param>
        /// <returns>True if the exchange was successful, false otherwise.</returns>
        private async Task<(bool, AuthResultDto?)> ExchangeTokenForZumoAsync(string azureAdToken, AuthResultDto authInfo)
        {
            try
            {
                // Create the token exchange request payload
                var tokenPayload = new
                {
                    access_token = azureAdToken
                };

                var jsonContent = JsonSerializer.Serialize(tokenPayload);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Call the Azure Mobile Apps /.auth/login/aad endpoint
                var response = await _httpClient.PostAsync("/.auth/login/aad", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"ZUMO token exchange failed: {response.StatusCode} - {errorContent}");
                    return (false,null);
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var zumoResponse = JsonSerializer.Deserialize<MobileServiceLoginDto>(responseContent);

                if (zumoResponse != null && !string.IsNullOrEmpty(zumoResponse.AuthenticationToken))
                {
                    authInfo.ZumoAuthToken = zumoResponse.AuthenticationToken;
                    authInfo.ZumoUserId = zumoResponse.User?.UserId;

                    // Set token expiration (ZUMO tokens typically expire in 24 hours)
                    authInfo.ZumoTokenExpiryTime = DateTimeOffset.UtcNow.AddHours(24);
                    return (true, authInfo);
                }

                return (false, null);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ZUMO token exchange error: {ex.Message}");
                return (false, null);
            }
        }

#if IOS
        private UIViewController GetCurrentViewController()
        {

            var window = UIApplication.SharedApplication.KeyWindow;
            var root = window.RootViewController;
            var current = root;

            while (current.PresentedViewController != null)
            {
                current = current.PresentedViewController;
            }

            return current;
        }
#endif

        public Task Clear()
        {

            return Task.CompletedTask;
        }

        public async Task SignOutAsync()
        {
            var accounts = await PCA.GetAccountsAsync();
            while (accounts.Any())
            {
                await PCA.RemoveAsync(accounts.FirstOrDefault());
                accounts = await PCA.GetAccountsAsync();

            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1416:This call site is reachable on: 'Android' 21.0 and later.", Justification = "MAUI we uses only latest platform API.")]
        private static AuthResultDto GetAuthResultDto(AuthenticationResult result)
        {
            if (result == null)
                return null;

            return new AuthResultDto()
            {
                UserId = result?.Account?.Username,
                DisplayName = result?.Account?.Username,
                AccessToken = result?.AccessToken,
                ExpiresIn = string.Empty,
                ExpiresOn = result?.ExpiresOn.ToString(),
                NotBefore = string.Empty,
                RefreshToken = string.Empty,
                Resource = string.Empty,
                //Scope = result.Scopes.FirstOrDefault(),
                TokenType = string.Empty,
                //Upn = result.Account.Username,
                OId = result?.UniqueId,
                IdToken = result?.IdToken ?? string.Empty,
            };
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1416:This call site is reachable on: 'Android' 21.0 and later.", Justification = "MAUI we uses only latest platform API.")]
        public async Task<AuthResultDto> GetUserAuth()
        {
            string[] scopes;
            AuthResultDto authResultData = new AuthResultDto();
            try
            {
                var accounts = await PCA.GetAccountsAsync();

                // scopes = new string[] { GraphConfiguration.GraphAPIUrlread };
                // var result = await PCA.AcquireTokenSilent(scopes, accounts.FirstOrDefault()).ExecuteAsync();
                // authResultData = GetAuthResultDto(result);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception getting graph token from graph api", exception);
            }
            return authResultData;
        }

        public async Task<string> GetUserSapId(string graphToken, string upn)
        {
            //string webAPIUrl = string.Concat(GraphConfiguration.UserGraphUrl, upn, AuthConstants.GraphUserSelectQuery);
            string sapId = string.Empty;
            // var uri = new Uri(webAPIUrl);
            // try
            // {
            //     HttpClient client = new HttpClient();
            //     client.DefaultRequestHeaders.Add("Authorization", "Bearer " + graphToken);
            //     var response = await client.GetAsync(uri);

            //     if (response.IsSuccessStatusCode)
            //     {
            //         var content = await response.Content.ReadAsStringAsync();
            //         sapId = JsonSerializer.Deserialize<GraphUserDto>(content).SapId;
            //     }
            // }
            // catch (Exception exception)
            // {
            //     Console.WriteLine("Exception getting user data from graph api", exception);
            // }
            return sapId;
        }

        private async Task<AuthenticationResult?> AcquireTokenAsync(string objectId)
        {
            var accounts = await PCA.GetAccountsAsync();
            var existingUser = accounts?
                               .FirstOrDefault(a => a.HomeAccountId?.ObjectId?.Contains(objectId) == true);
            try
            {
                return await PCA
                    .AcquireTokenSilent(_configuration.Scopes, existingUser)
                    .WithForceRefresh(true)
                    .ExecuteAsync();
            }
            catch (MsalUiRequiredException)
            {
                return await AcquireTokenInteractiveAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"MSAL Silent Error: {ex.Message}");
                return null;
            }
        }

        private async Task<AuthenticationResult?> AcquireTokenInteractiveAsync()
        {
            return await PCA.AcquireTokenInteractive(_configuration.Scopes)
                            .WithExtraScopesToConsent(_configuration.GraphApiScopes)
#if ANDROID
                           .WithParentActivityOrWindow(Platform.CurrentActivity)

#elif IOS
                            .WithParentActivityOrWindow((object)GetCurrentViewController())
#endif
                            .WithUseEmbeddedWebView(false)
                            .ExecuteAsync()
                            .ConfigureAwait(false);
        }
    }
}
