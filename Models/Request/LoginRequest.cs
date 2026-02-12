using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Pulse_MAUI.Models.Request
{
    public class LoginRequest
    {
        [JsonPropertyName("access_token")] 
        public string? AccessToken { get; set; }
    }
}
