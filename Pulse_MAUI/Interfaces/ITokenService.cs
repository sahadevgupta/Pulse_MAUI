using Pulse_MAUI.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pulse_MAUI.Interfaces
{
    public interface ITokenService
    {
        AuthInfoDto ReadToken(AuthResultDto authResult);
        AuthInfoDto ReadToken(string accessToken);
    }
}
