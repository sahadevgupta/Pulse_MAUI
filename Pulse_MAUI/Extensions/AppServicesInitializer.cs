using CommunityToolkit.Maui;
using Mopups.Interfaces;
using Mopups.Services;
using Pulse_MAUI.Configurations;
using Pulse_MAUI.Data;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models;
using Pulse_MAUI.Services;
using Pulse_MAUI.Services.Navigation;
using Pulse_MAUI.ViewModels.Common;

namespace Pulse_MAUI.Extensions
{
    public static class AppServicesInitializer
    {
        public static MauiAppBuilder RegisterAppServices(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<IActivityService, ActivityService>()
                            .AddSingleton<IActivitySearchService, ActivitySearchService>()
                            .AddSingleton<IAppConfiguration, AppConfiguration>()
                            .AddSingleton<IAppWorkflowManager, AppWorkflowManager>()
                            .AddSingleton<IAuthConfig, AppConfiguration>()
                            .AddSingleton<IAuthDriver, AuthDriver>()
                            .AddSingleton<IAuthService, AuthService>()
                            .AddSingleton<IBlobStorageService, BlobStorageService>()
                            .AddSingleton<IConnectivityService, ConnectivityService>()
                            .AddSingleton<IDialogService, DialogService>()
                            .AddSingleton<IDisciplineService, DisciplineService>()
                            .AddSingleton<IEngineerService, EngineerService>()
                            .AddSingleton<IEquipmentService, EquipmentService>()
                            .AddSingleton<IFileService, FileService>()
                            .AddSingleton<IItemService, ItemService>()
                            .AddSingleton<ILoginProvider, LoginProvider>()
                            .AddSingleton<ILookupService, LookupService>()
                            .AddSingleton<IDataManager, DataManager>()
                            .AddSingleton<INavigationService, NavigationService>()
                            .AddSingleton<IProjectServices, ProjectServices>()
                            .AddSingleton<IPullServices, PullServices>()
                            .AddSingleton<IPunchService, PunchService>()
                            .AddSingleton<IPunchSearchService, PunchSearchService>()
                            .AddSingleton<IPriorityService, PriorityService>()
                            .AddSingleton<ISecureStorageService, SecureStorageService>()
                            .AddSingleton<ISynchroniseService, SynchroniseService>()
                            .AddSingleton<ISyncLogService, SyncLogService>()
                            .AddSingleton<ISyncService, SyncService>()
                            .AddSingleton<ITokenService, TokenService>()
                            .AddSingleton<IUserService, UserService>()
                            .AddSingleton<IPopupNavigation>(MopupService.Instance)
#if ANDROID
                            .AddSingleton<ILoadingService, Pulse_MAUI.Platforms.Android.Services.LodingPageService>();
#elif IOS
                            .AddSingleton<ILoadingService, Pulse_MAUI.Platforms.iOS.Services.LodingPageService>();
#endif

            builder.Services.AddTransient<IViewModelParameters, ViewModelParameters>()
                            .AddTransient<IMediaService, MediaService>();
            //                .AddTransient<IApiServiceBaseParams, ApiServiceBaseParams>()
            //                .AddTransient<IDialogService, DialogService>();

            return builder;
        }
    }
}
