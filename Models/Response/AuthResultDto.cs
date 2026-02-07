using System;
using System.Collections.Generic;
using System.Text;

namespace Pulse_MAUI.Models.Response
{
    public record AuthResultDto
    {
        public string? UserId { get; set; }
        public string? DisplayName { get; set; }
        public string? AccessToken { get; set; }
        public string? ExpiresIn { get; set; }
        public string? ExpiresOn { get; set; }
        public string? ExtExpiresIn { get; set; }
        public string? NotBefore { get; set; }
        public string? RefreshToken { get; set; }
        public string? Resource { get; set; }
        public string? Scope { get; set; }
        public string? Token { get; set; }
        public string? TokenType { get; set; }
        public string? OId { get; set; }
        public string? AuthType { get; set; }
        public string? Upn { get; set; }

    }
}
