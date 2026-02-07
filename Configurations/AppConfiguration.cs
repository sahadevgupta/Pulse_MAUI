using Microsoft.Extensions.Configuration;
using Pulse_MAUI.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pulse_MAUI.Configurations
{
    public class AppConfiguration : IAppConfiguration
    {
        public AppConfiguration(IConfiguration configuration)
        {
            // API Configuration
            BaseUrl = configuration["ApiConfig:BaseUrl"] ?? throw new ArgumentNullException("ApiConfig:BaseUrl");

            // B2C Configuration
            Authority = configuration["B2CConfig:Authority"] ?? throw new ArgumentNullException("B2CConfig:ClientId");
            ClientId = configuration["B2CConfig:ClientId"] ?? throw new ArgumentNullException("B2CConfig:ClientId");
            RedirectUri = configuration["B2CConfig:RedirectUri"] ?? throw new ArgumentNullException("B2CConfig:RedirectUri");
            TenantId = configuration["B2CConfig:TenantId"] ?? throw new ArgumentNullException("B2CConfig:TenantId");
            //GraphApiScopes = new[] { configuration["B2CConfig:GraphApiScopes"] } ?? throw new ArgumentNullException("B2CConfig:GraphApiScopes");
            //Scopes = new[] { configuration["B2CConfig:Scopes"] } ?? throw new ArgumentNullException("B2CConfig:Scopes");

            GraphApiScopes = configuration
                    .GetSection("B2CConfig:GraphApiScopes")
                    .Get<string[]>() 
                    ?? throw new ArgumentNullException("GraphApiScopes");

            Scopes = configuration
                    .GetSection("B2CConfig:Scopes")
                    .Get<string[]>()
                    ?? throw new ArgumentNullException("Scopes");


        }
        public string RedirectUri { get; private set; }
        public string[] GraphApiScopes { get; private set; }
        public string[] Scopes { get; private set; }
        public string BaseUrl { get; private set; }

        public string TenantId { get; private set; }

        public string ClientId { get; private set; }

        public string Authority { get; private set; }
    }
}
