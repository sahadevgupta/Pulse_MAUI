using Pulse_MAUI.Extensions;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models.Response;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Pulse_MAUI.Services
{
    public class TokenService : ITokenService
    {
        /// <summary>
        ///     Reads the token.
        /// </summary>
        /// <returns>The token.</returns>
        /// <param name="accessToken">Access token.</param>
        public AuthInfoDto ReadToken(string accessToken)
        {
            try
            {
                var token = new JwtSecurityToken(accessToken);

                var authInfo = new AuthInfoDto
                {
                    Status = AuthInfoStatusType.Valid
                };

                var exp = token.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;
                authInfo.AccessTokenExpiresOn = Convert.ToInt32(exp).ToDate().AddSeconds(-30);
                authInfo.AccessToken = accessToken;
                authInfo.Email = token.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
                authInfo.Name = token.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
                authInfo.GivenName = token.Claims.FirstOrDefault(c => c.Type == "given_name")?.Value;
                authInfo.FamilyName = token.Claims.FirstOrDefault(c => c.Type == "family_name")?.Value;
                return authInfo;
            }
            catch (Exception)
            {
                return default(AuthInfoDto);
            }
        }

        /// <summary>
        ///     Reads the token.
        /// </summary>
        /// <returns>The token.</returns>
        /// <param name="authResult">Auth result.</param>
        public AuthInfoDto ReadToken(AuthResultDto authResult)
        {
            return ReadToken(authResult.AccessToken);
        }
    }
}
