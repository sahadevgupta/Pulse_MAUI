using CommunityToolkit.Maui;
using Mopups.Interfaces;
using Mopups.Services;
using PCATablet.Core.Data;
using Pulse_MAUI.Configurations;
using Pulse_MAUI.Data;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models;
using Pulse_MAUI.Repository;
using Pulse_MAUI.Services;
using Pulse_MAUI.ViewModels.Common;
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
                            .AddSingleton<IAppWorkflowManager, AppWorkflowManager>()
                            .AddSingleton<IAuthConfig, AppConfiguration>()
                            .AddSingleton<IAuthDriver, AuthDriver>()
                            .AddSingleton<IAuthService, AuthService>()
                            .AddSingleton<IBlobStorageService,BlobStorageService>()
                            .AddSingleton<IConnectivityService, ConnectivityService>()
                            .AddSingleton<IDialogService, DialogService>()
                            .AddSingleton<IEngineerService,EngineerService>()
                            .AddSingleton<IItemService,ItemService>()
                            .AddSingleton<ILoginProvider, LoginProvider>()
                            .AddSingleton<ILookupService,LookupService>()
                            .AddSingleton<IDataManager, DataManager>()
                            .AddSingleton<IProjectServices, ProjectServices>()
                            .AddSingleton<IPullServices,PullServices>()
                            .AddSingleton<IPunchService,PunchService>()
                            .AddSingleton<IPunchSearchService,PunchSearchService>()
                            .AddSingleton<ISecureStorageService,SecureStorageService>()
                            .AddSingleton<IShellNavigationService, ShellNavigationService>()
                            .AddSingleton<ISynchroniseService,SynchroniseService>()
                            .AddSingleton<ISyncLogService,SyncLogService>()
                            .AddSingleton<ISyncService,SyncService>()
                            .AddSingleton<ITokenService, TokenService>()
                            .AddSingleton<IUserService,UserService>()
                            .AddSingleton<IPopupNavigation>(MopupService.Instance)
#if ANDROID
                            .AddSingleton<ILoadingService, Pulse_MAUI.Platforms.Android.Services.LodingPageService>();
#elif IOS
                            .AddSingleton<ILoadingService, Pulse_MAUI.Platforms.iOS.Services.LodingPageService>();
#endif

            builder.Services.AddTransient<IViewModelParameters, ViewModelParameters>();
            //                .AddTransient<IApiServiceBaseParams, ApiServiceBaseParams>()
            //                .AddTransient<IDialogService, DialogService>();

            return builder;
        }

        public static MauiAppBuilder RegisterDBRepositories(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<IDBAccessRepository<Activity>, DBAccessRepository<Activity>>()
                            .AddSingleton<IDBAccessRepository<ActivityTask>, DBAccessRepository<ActivityTask>>()
                            .AddSingleton<IDBAccessRepository<CommissioningSystem>, DBAccessRepository<CommissioningSystem>>()
                            .AddSingleton<IDBAccessRepository<Component>, DBAccessRepository<Component>>()
                            .AddSingleton<IDBAccessRepository<Discipline>, DBAccessRepository<Discipline>>()
                            .AddSingleton<IDBAccessRepository<Engineer>, DBAccessRepository<Engineer>>()
                            .AddSingleton<IDBAccessRepository<Equipment>, DBAccessRepository<Equipment>>()
                            .AddSingleton<IDBAccessRepository<Item>, DBAccessRepository<Item>>()
                            .AddSingleton<IDBAccessRepository<Lookup>, DBAccessRepository<Lookup>>()
                            .AddSingleton<IDBAccessRepository<Priority>, DBAccessRepository<Priority>>()
                            .AddSingleton<IDBAccessRepository<Project>, DBAccessRepository<Project>>()
                            .AddSingleton<IDBAccessRepository<PunchItem>, DBAccessRepository<PunchItem>>()
                            .AddSingleton<IDBAccessRepository<Unit>, DBAccessRepository<Unit>>()
                            .AddSingleton<IDBAccessRepository<User>, DBAccessRepository<User>>();

            return builder;
        }
    }
}
