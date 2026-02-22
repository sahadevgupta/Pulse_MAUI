using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pulse_MAUI.Helpers
{
    public static class AppHelpers
    {
        public static bool IsLoggedIn
        {
            get => Preferences.Get(nameof(IsLoggedIn), false);
            set => Preferences.Set(nameof(IsLoggedIn), value);
        }

        public static string AzureServiceUrl
        {
            get => Preferences.Get(nameof(AzureServiceUrl), "https://www.syncservice.com");
            set => Preferences.Set(nameof(AzureServiceUrl), value);
        }

        public static string AppTitle
        {
            get => Preferences.Get(nameof(AppTitle), "Pulse");
            set => Preferences.Get(nameof(AppTitle), value);
        }

        public static string BlobStorageName
        {
            get => Preferences.Get(nameof(BlobStorageName), string.Empty);
            set => Preferences.Get(nameof(BlobStorageName), value);
        }
        public static string SyncDate
        {
            get => Preferences.Get(nameof(SyncDate), string.Empty);
            set => Preferences.Get(nameof(SyncDate), value);
        }

        public static bool UseLocationServices
        {
            get
            {
                ISettingProvider settingProvider = IPlatformApplication.Current?.Services.GetService<ISettingProvider>()!;
                return settingProvider.UseLocationServices();
            }
        }

        public static IDBAccessRepository<T> GetRepository<T>() where T : new()
        {
            var repoType = typeof(IDBAccessRepository<>).MakeGenericType(typeof(T));
            var repo = ServiceHelper.GetService(repoType) as IDBAccessRepository<T>;

            if (repo == null)
                throw new InvalidOperationException($"Repository not configured for type '{typeof(T).Name}'");

            return repo;
        }
    }
}
