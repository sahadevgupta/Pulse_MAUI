using Newtonsoft.Json;
using Pulse_MAUI.Constants;
using Pulse_MAUI.Models.Response;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;

namespace Pulse_MAUI.Data
{
    public class AuthHeaderHandler : DelegatingHandler
    {
        public static string AccessToken { get; set; }

        /// <summary>
        /// Overrides the send async method. Currently not used.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="request">Request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await SecureStorage.GetAsync(ADConstants.AuthResultKey);

            if (!string.IsNullOrEmpty(token))
            {
                var details = JsonConvert.DeserializeObject<AuthResultDto>(token);
                request.Headers.Add("X-ZUMO-AUTH", details.ZumoAuthToken);
            }

            if (request.RequestUri.AbsoluteUri.Contains("&$count=true"))
            {
               var newUri = request.RequestUri.AbsoluteUri.Replace("&$count=true", "");
                request.RequestUri = new Uri(newUri);
            }

            var req = request;
            var id = Guid.NewGuid().ToString();
            var msg = $"[{id} -   Request]";
            StringBuilder apiDetails = new StringBuilder();



            Debug.WriteLine($"{msg}========Start==========");
            Debug.WriteLine($"{msg} {req.Method} {req.RequestUri?.PathAndQuery} {req.RequestUri?.Scheme}/{req.RequestUri?.Host}");
            Debug.WriteLine($"{msg} Host: {req.RequestUri?.Scheme}://{req.RequestUri?.Host}");

            apiDetails.Append($"{"Starting Api Call "}{req.Method}");
            apiDetails.Append($"{"RequestUri "}{req.RequestUri?.PathAndQuery}");

            foreach (var header in req.Headers)
                Debug.WriteLine($"{msg} {header.Key}: {string.Join(", ", header.Value)}");

            if (req.Content != null)
            {
                foreach (var header in req.Content.Headers)
                {
                    Debug.WriteLine($"{msg} {header.Key}: {string.Join(", ", header.Value)}");
                    apiDetails.Append($"{"Header Key "}{header.Key}");
                    apiDetails.Append($"{"Header Value "}{header.Value}");
                }

                if (req.Content is StringContent)
                {
                    var result = await req.Content.ReadAsStringAsync();
                    var reqString = JsonConvert.SerializeObject(result);

                    Debug.WriteLine($"{msg} Content:");
                    Debug.WriteLine($"{msg} {string.Join("", reqString)}");
                    apiDetails.Append($"{"Request "}{reqString}");
                }
                else
                {
                    string body = await request.Content.ReadAsStringAsync();

                    var reqString = JsonConvert.SerializeObject(body);

                    Debug.WriteLine($"{msg} Content:");
                    Debug.WriteLine($"{msg} {string.Join("", reqString)}");
                    apiDetails.Append($"{"Request "}{reqString}");
                }
            }

            var start = DateTime.Now;

            var a = request.RequestUri;

            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            var end = DateTime.Now;

            Debug.WriteLine($"{msg} Duration: {end - start}");
            Debug.WriteLine($"{msg}==========End==========");
            apiDetails.Append($"{msg} Duration: {end - start}");

            msg = $"[{id} - Response]";
            Debug.WriteLine($"{msg}=========Start=========");
            apiDetails.Append($"{msg} [{id} - Response]");

            var resp = response;
            foreach (var header in resp.Headers)
            {
                Debug.WriteLine($"{msg} {header.Key}: {string.Join(", ", header.Value)}");
                apiDetails.Append($"{"Header Key "}{header.Key}");
                apiDetails.Append($"{"Header Value "}{header.Value}");
            }
            try
            {
                if (resp.Content != null)
                {
                    var result = await resp.Content.ReadAsStringAsync(cancellationToken);
                    start = DateTime.Now;
                    Debug.WriteLine($"{msg} Content:{result.ToString()}");
                    apiDetails.Append($"{"Content "}{result.ToString()}");
                    end = DateTime.Now;
                    Debug.WriteLine($"{msg} Duration: {end - start} Time span");
                    apiDetails.Append($"{"Duration "}{end - start} Time span");
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine("Failed - HandleException [{exceptionName}] \n{exceptionToString}", exception.GetType().Name, exception.ToString());
            }
            Debug.WriteLine($"{msg}==========End==========");
            Console.WriteLine(apiDetails);
            return response;
        }


    }
}
