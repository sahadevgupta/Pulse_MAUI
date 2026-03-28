using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Pulse_MAUI.Models.Response
{
    public sealed class ServiceResponse<T>
    {
        [JsonPropertyName("data")]
        public T? Data { get; set; }

        [JsonPropertyName("code")]
        public string? ErrorCode { get; set; }

        [JsonPropertyName("errorMessage")]
        public string? ErrorMessage { get; set; }

        [JsonPropertyName("success")]
        public bool Status { get; set; }
    }
}
