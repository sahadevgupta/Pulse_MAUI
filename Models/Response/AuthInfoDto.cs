using System;
using System.Collections.Generic;
using System.Text;

namespace Pulse_MAUI.Models.Response
{
    public enum AuthInfoStatusType
    {
        None,
        Valid,
        Failure,
        RequiredInteractive
    }
    public class AuthInfoDto
    {
        public string? AccessToken { get; set; }
        public string? IdToken { get; set; }
        public DateTime AccessTokenExpiresOn { get; set; }
        public string? Email { get; set; }
        public string? FamilyName { get; set; }
        public string? GivenName { get; set; }
        public string? Name { get; set; }
        public Guid Oid { get; set; }
        public AuthInfoStatusType Status { get; set; }
        public string? UserId { get; set; }
        public string? ZumoUserId { get; set; }
        public string? ZumoAuthToken { get; set; }
        public DateTimeOffset ZumoTokenExpiryTime { get; set; }
    }
}
