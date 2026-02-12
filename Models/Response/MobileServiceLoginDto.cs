using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Pulse_MAUI.Models.Response
{
    public record MobileServiceLoginDto
    {
        [JsonPropertyName("authenticationToken")]
        public string? AuthenticationToken { get; set; }

        [JsonPropertyName("user")]
        public UserDto? User { get; set; }
    }
}
