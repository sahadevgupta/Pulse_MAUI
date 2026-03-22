using Microsoft.Maui.ApplicationModel;
using PCATablet.Core.Data;
using Pulse_MAUI.Configurations;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pulse_MAUI.Extensions
{
    public static class AppInitializer
    {
        public static MauiAppBuilder InitializeApp(this MauiAppBuilder builder)
        {
            builder
                .RegisterAppServices()
                .ViewModelInit();

            IAppConfiguration Configuration = builder.Services.BuildServiceProvider().GetRequiredService<IAppConfiguration>();
            builder.RefitClientInit(Configuration);

            return builder;
        }
    }
}
