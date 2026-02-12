using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models.Response;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Pulse_MAUI.Services
{
    public abstract class ApiServiceBase
    {
        protected IAuthService AuthService;
        public ApiServiceBase(IAuthService authService)
        {
            AuthService = authService;
        }
        protected void HandleException(Exception exception)
        {
            //Handle the exception
            Debug.WriteLine($"ApiServiceBase HandleException [{exception.GetType().Name}] \n{exception.ToString()}");
        }
        protected async Task<Dictionary<string, string>> GetHeader()
        {

            var header = new Dictionary<string, string> { };
            try
            {

                var authInfo = await AuthService.ValidateAuth("https://pulseargwebappmobile.azurewebsites.net");
                if (authInfo != null)
                {
                    header.Add("X-ZUMO-AUTH", authInfo.ZumoAuthToken);
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
            return header;
        }
        public IEnumerable<T> SetResponse<T>(IEnumerable<T> list, ServiceResponse<IEnumerable<T>> response)
        {
            if (response != null)
            {
                list = SetResponseData(list, response);
            }
            return list;
        }
        public T SetResponse<T>(T modelObject, ServiceResponse<T> response)
        {
            if (response != null)
            {
                modelObject = SetResponseData(modelObject, response);
            }
            return modelObject;
        }
        public bool SetSaveResponse(bool IsSaved, ServiceResponse<bool?> response)
        {
            if (response != null)
            {
                ErrorResponse(response);
                if (response.Data != null)
                {
                    IsSaved = response.Data.Value;
                }
            }
            return IsSaved;
        }
        private T SetResponseData<T>(T modelObject, ServiceResponse<T> response)
        {
            if (response.Data != null)
            {
                modelObject = response.Data;
            }
            else
            {
                ErrorResponse(response);
            }

            return modelObject;
        }
        private IEnumerable<T> SetResponseData<T>(IEnumerable<T> list, ServiceResponse<IEnumerable<T>> response)
        {
            if (response.Data != null)
            {
                list = response.Data;
            }
            else
            {
                ErrorResponse(response);
            }

            return list;
        }

        public void ErrorResponse<T>(ServiceResponse<T> response)
        {
            // if (response.Error != null)
            // {
            //     Log.Error(CorrelationId + response.Error.ErrorCode.ToString(), response.Error.Message);
            //     SetException(response);
            //     return;
            // }
        }
    }
}
