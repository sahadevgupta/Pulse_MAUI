using CommunityToolkit.Maui;
using Mopups.Interfaces;
using Mopups.Services;
using PCATablet.Core.Data;
using Pulse_MAUI.Configurations;
using Pulse_MAUI.Data;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pulse_MAUI.Extensions
{
    public static class AppServicesInitializer
    {
        public static MauiAppBuilder RegisterAppServices(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<IActivityService, ActivityService>()
                            .AddSingleton<IActivitySearchService, ActivitySearchService>()
                            .AddSingleton<IAppConfiguration, AppConfiguration>()
                            .AddSingleton<IAuthConfig, AppConfiguration>()
                            .AddSingleton<IAuthDriver, AuthDriver>()
                            .AddSingleton<IAuthService, AuthService>()
                            .AddSingleton<IConnectivityService, ConnectivityService>()
                            .AddSingleton<IDialogService, DialogService>()
                            .AddSingleton<IEngineerService,EngineerService>()
                            .AddSingleton<ILoginProvider, LoginProvider>()
                            .AddSingleton<IDataManager, DataManager>()
                            .AddSingleton<IProjectServices, ProjectServices>()
                            .AddSingleton<ISecureStorageService,SecureStorageService>()
                            .AddSingleton<IShellNavigationService, ShellNavigationService>()
                            .AddSingleton<ITokenService, TokenService>()
                            .AddSingleton<IPopupNavigation>(MopupService.Instance)
#if ANDROID
                            .AddSingleton<ILoadingService, Pulse_MAUI.Platforms.Android.Services.LodingPageService>();
#elif IOS
                            .AddSingleton<ILoadingService, Pulse_MAUI.Platforms.iOS.Services.LodingPageService>();
#endif

            //builder.Services.AddTransient<IViewModelParameters, ViewModelParameters>()
            //                .AddTransient<IApiServiceBaseParams, ApiServiceBaseParams>()
            //                .AddTransient<IDialogService, DialogService>();

            return builder;
        }
    }
}
