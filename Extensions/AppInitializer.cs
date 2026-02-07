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

        private static MauiAppBuilder RegisterAppServices(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<IAppConfiguration, AppConfiguration>()
                            .AddSingleton<IAuthConfig, AppConfiguration>()
                            .AddSingleton<IAuthDriver, AuthDriver>()
                            .AddSingleton<ILoginProvider, LoginProvider>()
                            .AddSingleton<IDataManager,DataManager>()
                            .AddSingleton<IProjectServices, ProjectServices>()
                            .AddSingleton<IShellNavigationService, ShellNavigationService>();

            //builder.Services.AddTransient<IViewModelParameters, ViewModelParameters>()
            //                .AddTransient<IApiServiceBaseParams, ApiServiceBaseParams>()
            //                .AddTransient<IDialogService, DialogService>();

            return builder;
        }
    }
}
