using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Pulse_MAUI.Constants;
using Pulse_MAUI.Helpers.CustomExceptions;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models.Response;
using static SkiaSharp.HarfBuzz.SKShaper;

namespace Pulse_MAUI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConnectivityService _connectivity;
        private readonly IAuthDriver _authDriver;
        readonly ITokenService _tokenService;
        readonly ISecureStorageService _secureStorageService;
        private AuthInfoDto? _authInfo;

        public AuthService(
            IAuthDriver authDriver,
            IConnectivityService connectivity,
            ITokenService tokenService,
            ISecureStorageService secureStorageService)
        {
            _authDriver = authDriver;
            _connectivity = connectivity;
            _tokenService = tokenService;
            _secureStorageService = secureStorageService;
        }

        public async Task<AuthInfoDto> Auth(string azureMobileServiceUrl)
        {
            try
            {
                Console.WriteLine("Auth Interactivly");

                await _connectivity.CheckConnected();

                Console.WriteLine(">>> IAzureActiveDirectoryDriver.AuthenticateUser ");

                var authResult = await _authDriver.AuthenticateUser(azureMobileServiceUrl);
                Console.WriteLine("<<< IAzureActiveDirectoryDriver.AuthenticateUser ");

                await SetAuthData(authResult);
                _authInfo = _tokenService.ReadToken(authResult.AccessToken);

                _authInfo.IdToken = authResult.IdToken;
                _authInfo.UserId = authResult.UserId;
                _authInfo.ZumoAuthToken = authResult.ZumoAuthToken;
                _authInfo.ZumoTokenExpiryTime = authResult.ZumoTokenExpiryTime;
                _authInfo.ZumoUserId = authResult.ZumoUserId;

                return _authInfo;
            }
            catch (Exception exception)
            {
                return ProcessException<AuthInfoDto>("Fail to authenticate user.", exception);
            }
        }

        public async Task<AuthInfoDto> ValidateAuth(string azureMobileServiceUrl)
        {
            try
            {
                if (_authInfo == null)
                {
                    _authInfo = _tokenService.ReadToken(await GetAuthData());
                }

                if (_authInfo == null)
                {
                    throw new AuthServiceInteractiveLogonRequiredException("No cached credentials available");
                }

                if (!IsAuthInfoValid(_authInfo))
                {
                    await _connectivity.CheckConnected();

                    var authResult = await _authDriver.AuthenticateUser(azureMobileServiceUrl);

                    await SetAuthData(authResult);

                    _authInfo = _tokenService.ReadToken(authResult.AccessToken);
                }

                return _authInfo;
            }
            catch (Exception exception)
            {
                return ProcessException<AuthInfoDto>("Fail to authenticate user silently.", exception);
            }
        }

        public Task Logoff()
        {
            try
            {
                Console.WriteLine("Logoff");

                _authInfo = null;
                _authDriver.Clear();
                _authDriver.SignOutAsync();
                return Task.CompletedTask;
            }
            catch (Exception exception)
            {
                return ProcessException<Task>("Fail to logoff", exception);
            }
        }

        public T ProcessException<T>(string message, Exception exception, bool doThrow = true)
        {
            if (exception is NotConnectedException)
            {
                ThrowHandledTransformation(new AuthServiceNotConnectedException(message, exception));
            }

            if (exception is AuthDriverUserCanceledException)
            {
                ThrowHandledTransformation(new AuthServiceUserCanceledException(message, exception));
            }

            if (exception is AuthDriverUIRequiredException)
            {
                ThrowHandledTransformation(new AuthServiceInteractiveLogonRequiredException(message, exception));
            }

            if (exception is AuthServiceNoCachedInfoException)
            {
                ThrowHandledTransformation(new AuthServiceInteractiveLogonRequiredException(message, exception));
            }

            if (exception is AuthDriverAadCallFailureException)
            {
                ThrowHandledTransformation(new AuthServiceInteractiveLogonRequiredException(message, exception));
            }

            if (exception is AuthDriverAadCallFailureException)
            {
                ThrowHandledTransformation(new AuthServiceFailedToLogonUserException(message, exception));
            }

            if (exception is MsalClientException)
            {
                ThrowHandledTransformation(new AuthServiceUserCanceledException(message, exception));
            }

            ThrowHandledTransformation(new AuthServiceException(message, exception));

            return default(T);
        }

        private void ThrowHandledTransformation(AuthServiceException exception)
        {
            Console.WriteLine($"AuthService Exception Handling Handled {exception.GetType().Name} throwing {exception.InnerException?.GetType().Name}");

            throw exception;
        }

        private async Task<AuthResultDto> GetAuthData()
        {
            var cacheAuthResult = await _secureStorageService.GetAsync<AuthResultDto>(ADConstants.AuthResultKey);

            if (cacheAuthResult == null)
            {
                throw new AuthServiceNoCachedInfoException();
            }

            Console.WriteLine($"GetAuthData {cacheAuthResult}");
            return cacheAuthResult;

        }

        public async Task<bool> IsTokenAvailable()
        {
            return await _secureStorageService.GetAsync<AuthResultDto>(ADConstants.AuthResultKey) != null;
        }

        private bool IsAuthInfoValid(AuthInfoDto authInfo)
        {
            bool ret = true;

            if (authInfo == null)
            {
                Console.WriteLine($"IsAuthInfoValid [false] Authinfo is null");
                ret = false;
            }
            else if (authInfo.AccessTokenExpiresOn < DateTime.UtcNow)
            {
                Console.WriteLine($"IsAuthInfoValid [false] authInfo.AccessTokenExpiresOn[{authInfo.AccessTokenExpiresOn}] < DateTime.UtcNow[{DateTime.UtcNow}]", authInfo.AccessTokenExpiresOn, DateTime.UtcNow);

                ret = false;
            }

            Console.WriteLine($"IsAuthInfoValid [{ret}]");

            return ret;
        }

        private async Task SetAuthData(AuthResultDto authResult)
        {
            Preferences.Set(ADConstants.ObjectId, authResult.OId);
            await _secureStorageService.SetAsync(ADConstants.AuthResultKey, JsonConvert.SerializeObject(authResult));
        }
    }
}
