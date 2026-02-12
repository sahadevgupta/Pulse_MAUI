using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Pulse_MAUI.Models.Response
{
    public record UserDto
    {
        [JsonPropertyName("userId")]
        public string? UserId { get; set; }
    }
}
