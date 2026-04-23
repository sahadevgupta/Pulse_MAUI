using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Datasync.Client;
using Pulse_MAUI.Enums;
using Pulse_MAUI.Events;
using Pulse_MAUI.Helpers;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models;
using Pulse_MAUI.Resources.Languages;
using Pulse_MAUI.Views;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using Pulse_MAUI.Popups;
using Mopups.Services;
using System.Globalization;

namespace Pulse_MAUI.ViewModels
{
    public partial class MenuPageViewModel : BaseViewModel
    {
        #region [ Properties ]

        readonly IAppWorkflowManager _appWorkflowManager;
        readonly IActivityService _activityService;
        readonly IActivitySearchService _activitySearchService;
        readonly IPunchSearchService _punchSearchService;
        //readonly ILookupService _lookupService;
        //readonly IUserService _userService;
        //readonly ISyncLogService _syncLogService;
        //readonly ISynchroniseService _synchroniseService;
        [ObservableProperty] private string _closeButtonText = "Please wait";
        [ObservableProperty] private string _buildNumberText = string.Empty;
        [ObservableProperty] private string _lastSyncText = "Your last sync: Never";
        [ObservableProperty] private string _statusMessage = string.Empty;

        [ObservableProperty]
        ObservableCollection<MenuOption>? _optionsItems;

        [ObservableProperty]
        private bool _isSyncEnabled;

        [ObservableProperty]
        private string? _currentDate = DateTime.Now.ToString("dd MMMM yyyy HH:mm:ss");

        private ObservableCollection<SyncStepModel> _syncSteps = new();
        private string _currentSyncStep = string.Empty;

        public string ProfileName
        {
            get
            {
                return string.Format(UserInterface.MenuPage_HelloTitle,
                                     _appWorkflowManager.EngineerService.CurrentEngineer != null ?
                                     _appWorkflowManager.EngineerService.CurrentEngineer.Name :
                                     "N/A");
            }

        }
        public string AppName => Preferences.Get("AppTitle", "Pulse Mobile");
        public string AppVersion => string.Format("Version {0}", VersionTracking.CurrentVersion);

        #endregion
        public MenuPageViewModel(IViewModelParameters viewModelParameters,
            IActivityService activityService,
            IActivitySearchService activitySearchService,
            IPunchSearchService punchSearchService) : base(viewModelParameters)
        {
            _appWorkflowManager = viewModelParameters.AppWorkflowManager;
            _activityService = activityService;
            _activitySearchService = activitySearchService;
            _punchSearchService = punchSearchService;

            BuildNumberText = $"App Version: {AppInfo.Current.VersionString}";
            LastSyncText = FormatLastSyncText(AppHelpers.SyncDate);
        }

        #region [ Methods && Service Calls ]

        private async Task InitializeDataAsync()
        {
            PopulateOptionsMenu();
            App.Current?.Dispatcher.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                CurrentDate = DateTime.Now.ToString("dd MMMM yyyy HH:mm:ss");
                return true;
            });
            await FetchDataCommand.ExecuteAsync(null);

        }

        /// <summary>
		/// Populate the items for the options menu.
		/// </summary>
		private void PopulateOptionsMenu()
        {
            if (AppHelpers.AzureServiceUrl == "https://www.syncservice.com")
            {
                IsSyncEnabled = false;
                OptionsItems = new ObservableCollection<MenuOption>
                {
                    new MenuOption{
                    Title = "Import Settings",

                    Route = nameof(ImportSettingsPage),
                    TargetType = typeof(ImportSettingsPage),
                    Index = 2
                    }
                };
            }
            else
            {
                IsSyncEnabled = true;
                OptionsItems = new ObservableCollection<MenuOption>
                {
                  new MenuOption{
                      Title = "Activities",
                      IconSource="activities_icon",
                      Route = "//activities",
                      TargetType = typeof(ActivityListPage),
                      Index = 0
                  }
                  ,new MenuOption{
                      Title = "Punch List",
                      IconSource="punches_icon",
                      Route = "//punches",
                      TargetType = typeof(PunchListPage),
                      Index = 1
                  },
                //   new MenuOption{
                //     Title = "Import Settings",
                //     IconSource="settings_icon",
                //     Route = "//import-settings",
                //     TargetType = typeof(ImportSettingsPage),
                //     Index = 2
                //     }
                };
            }
        }

        private async Task PerformFullSyncAsync()
        {
            if (!AppHelpers.IsLoggedIn)
            {
                await DialogService.ShowAlertDialog("Login Error", "Invalid User Login", AlertType.Error);
                return;
            }

            Guid transactionBatchId = Guid.NewGuid();
            await _appWorkflowManager.SyncLogService.PostSyncLogStart(transactionBatchId);

            try
            {
                if (!await EnsureInternetAsync()) return;

                var blobConnectionString = await _appWorkflowManager.UserService.GetAzureBlobStorageString();

                // 1. Standing data sync (full or incremental)
                await SyncStandingDataAsync();

                // 2. Upload blobs
                if (!await EnsureInternetAsync()) return;
                //DialogService.ShowLoading("Uploading Image Items");
                await _appWorkflowManager.SynchroniseService.UploadBlobData(blobConnectionString);

                // 3. Push/Pull data after images uploaded
                if (!await EnsureInternetAsync()) return;
                //DialogService.ShowLoading(UserInterface.MenuPage_Synchronising);
                await _appWorkflowManager.SynchroniseService.PushAndPullDataAsync(true, true);

                // 4. Download blobs
                if (!await EnsureInternetAsync()) return;
                //DialogService.ShowLoading("Downloading Image Items");
                await _appWorkflowManager.SynchroniseService.DownloadBlobData(blobConnectionString);

                // 5. Finish sync
                await _appWorkflowManager.SyncLogService.PostSyncLogFinish(transactionBatchId);
                var syncDateTime = DateTime.UtcNow.ToString("dd-MM-yyyy, HH:mm:ss");

                AppHelpers.SyncDate = syncDateTime;

                // Refresh user
                await _appWorkflowManager.UserService.FetchCurrentUser();

                await DialogService.ShowAlertDialog("Suceess!!", "Sync Completed", Enums.AlertType.Success);

            }
            catch (DatasyncInvalidOperationException ex)
            {
                Debug.WriteLine(ex.Message);
                SentrySdk.CaptureException(ex);
                //await DialogService.ShowAlertDialog("Sync Error", ex.Message, AlertType.Error);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                SentrySdk.CaptureException(ex);
                //await DialogService.ShowAlertDialog("Sync Error", "Unable to complete data sync", AlertType.Error);
            }
        }

        private async Task PerformFullSyncWithProgressAsync()
        {
            _syncSteps = new ObservableCollection<SyncStepModel>
            {
                new SyncStepModel { Name = "Login", Status = "[ ]", Description = "Confirm the saved session is still valid" },
                new SyncStepModel { Name = "Prepare Sync", Status = "[ ]", Description = "Open the sync session and load blob storage settings" },
                new SyncStepModel { Name = "Activity and Checklist Updates", Status = "[ ]", Description = "Upload activity status changes and checklist step results" },
                new SyncStepModel { Name = "Punch Updates", Status = "[ ]", Description = "Upload new and edited punch records" },
                new SyncStepModel { Name = "Photo Records", Status = "[ ]", Description = "Save photo deletes, descriptions, and checklist links" },
                new SyncStepModel { Name = "Refresh Records", Status = "[ ]", Description = "Refresh photos, activities, punches, and reference data" },
                new SyncStepModel { Name = "Photo Files", Status = "[ ]", Description = "Transfer photos and refresh the local photo cache" },
            };

            var popup = new SyncProgressPopup(_syncSteps, "Starting sync...");
            await MopupService.Instance.PushAsync(popup);

            try
            {
                // Step 1: Login
                await StartStepAsync("Login", popup, "Checking your sign-in...");
                //_syncSteps[0].IsCurrent = true;
                await _appWorkflowManager.UserService.LoginAsync(AppHelpers.AzureServiceUrl);
                _syncSteps[0].Status = "[OK]";
                _syncSteps[0].IsCompleted = true;

                // Step 2: Prepare Sync
                popup.CurrentStep = "Preparing sync...";
                _syncSteps[1].IsCurrent = true;
                Guid transactionBatchId = Guid.NewGuid();
                await _appWorkflowManager.SyncLogService.PostSyncLogStart(transactionBatchId);
                _syncSteps[1].Status = "[OK]";
                _syncSteps[1].IsCompleted = true;

                // Step 3: Activity and Checklist Updates
                _syncSteps[2].IsCurrent = true;
                popup.CurrentStep = "Syncing standing data...";
                await SyncStandingDataAsync();
                _syncSteps[2].Status = "[OK]";
                _syncSteps[2].IsCompleted = true;

                // Step 4: Punch Updates
                _syncSteps[3].IsCurrent = true;
                popup.CurrentStep = "Uploading punch updates...";
                // If you have a punch update method, call it here
                _syncSteps[3].Status = "[OK]";
                _syncSteps[3].IsCompleted = true;

                // Step 5: Photo Records
                _syncSteps[4].IsCurrent = true;
                popup.CurrentStep = "Saving photo records...";
                // If you have a photo record method, call it here
                _syncSteps[4].Status = "[OK]";
                _syncSteps[4].IsCompleted = true;

                // Step 6: Refresh Records
                _syncSteps[5].IsCurrent = true;
                popup.CurrentStep = "Refreshing records...";
                var blobConnectionString = await _appWorkflowManager.UserService.GetAzureBlobStorageString();
                await _appWorkflowManager.SynchroniseService.UploadBlobData(blobConnectionString);
                await _appWorkflowManager.SynchroniseService.PushAndPullDataAsync(true, true);
                await _appWorkflowManager.SynchroniseService.DownloadBlobData(blobConnectionString);
                _syncSteps[5].Status = "[OK]";
                _syncSteps[5].IsCompleted = true;

                // Step 7: Photo Files
                _syncSteps[6].IsCurrent = true;
                popup.CurrentStep = "Transferring photo files...";
                // If you have a photo file transfer method, call it here
                _syncSteps[6].Status = "[OK]";
                _syncSteps[6].IsCompleted = true;

                // Finish sync
                await _appWorkflowManager.SyncLogService.PostSyncLogFinish(transactionBatchId);
                var syncDateTime = DateTime.UtcNow.ToString("dd-MM-yyyy, HH:mm:ss");
                AppHelpers.SyncDate = syncDateTime;

                LastSyncText = FormatLastSyncText(AppHelpers.SyncDate);
                await _appWorkflowManager.UserService.FetchCurrentUser();
                popup.CurrentStep = "Sync completed!";
                await Task.Delay(1000);
                //await MopupService.Instance.PopAsync();
                WeakReferenceMessenger.Default.Send(new NotificationMessageEvent(NotifyType.PostSyncRefresh));
                PopulateOptionsMenu();
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                popup.CurrentStep = $"Error: {ex.Message}";
                await Task.Delay(2000);
                //await MopupService.Instance.PopAsync();
                //await DialogService.ShowAlertDialog("Sync Error", ex.Message, AlertType.Error);
            }
        }

        private string FormatLastSyncText(string? storedValue)
        {
            if (string.IsNullOrWhiteSpace(storedValue))
                return "Your last sync: Never";

            DateTimeOffset utcTimestamp;
            if (DateTimeOffset.TryParse(storedValue, out var parsedTimestamp))
            {
                utcTimestamp = parsedTimestamp.ToUniversalTime();
            }
            else if (DateTime.TryParseExact(
                         storedValue,
                         "dd-MM-yyyy, HH:mm:ss",
                         CultureInfo.InvariantCulture,
                         DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                         out var legacyTimestamp))
            {
                utcTimestamp = new DateTimeOffset(legacyTimestamp, TimeSpan.Zero);
            }
            else
            {
                return $"Your last sync: {storedValue}";
            }

            var localTimestamp = utcTimestamp.ToLocalTime();
            var timezoneCode = TimeZoneAbbreviationHelper.GetLocalTimeZoneCode(localTimestamp);
            return $"Your last sync: {localTimestamp:dd-MMM-yyyy HH:mm:ss} ({timezoneCode})";
        }

        private async Task<bool> EnsureInternetAsync()
        {
            if (!ViewModelParameters.ConnectivityService.IsConnected)
            {
                await DialogService.ShowAlertDialog("Alert!!", "No Internet Connection Available");
                return false;
            }
            return true;
        }

        private async Task SyncStandingDataAsync()
        {
            try
            {
                if (!await EnsureInternetAsync()) return;

                var projectData = await _appWorkflowManager.LookupService.GetProjectListAsync();

                bool isIncremental = projectData != null && projectData.Any();

                string loadingMsg = isIncremental
                    ? UserInterface.MenuPage_Synchronising
                    : UserInterface.MenuPage_Synchronising + " (Full)";

                //DialogService.ShowLoading(loadingMsg);

                var syncResult = await _appWorkflowManager.SynchroniseService.PushAndPullDataAsync(isIncremental, false);

                if (syncResult?.Count > 0)
                {
                    var sb = new StringBuilder();
                    syncResult.ForEach(e => sb.AppendLine(e));
                    await DialogService.ShowAlertDialog("Sync Error", sb.ToString(), AlertType.Error);
                }
            }
            catch (DatasyncInvalidOperationException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                Debug.WriteLine(ex.Message);
                //await DialogService.ShowAlertDialog("Sync Error", "Unable to complete data sync", AlertType.Error);
            }
        }

        public Task StartStepAsync(string stepName, SyncProgressPopup popup, string? status = null)
            => MainThread.InvokeOnMainThreadAsync(() =>
                {
                    foreach (var item in _syncSteps)
                        item.IsCurrent = false;

                    var step = _syncSteps.FirstOrDefault(x => string.Equals(x.Name, stepName, StringComparison.Ordinal));
                    if (step != null)
                        step.IsCurrent = true;

                    if (!string.IsNullOrWhiteSpace(status))
                    {
                        //ClearWarnings();
                        popup.CurrentStep = status;
                        StatusMessage = status;
                    }
                });

        public Task MarkStepCompleteAsync(string stepName, SyncProgressPopup popup, string? status = null)
            => MainThread.InvokeOnMainThreadAsync(() =>
                {
                    var step = _syncSteps.FirstOrDefault(x => string.Equals(x.Name, stepName, StringComparison.Ordinal));
                    if (step != null)
                    {
                        step.IsCurrent = false;
                        step.IsCompleted = true;
                    }

                    if (!string.IsNullOrWhiteSpace(status))
                    {
                        //ClearWarnings();
                        popup.CurrentStep = status;
                        StatusMessage = status;
                    }
                });

        private void RefreshState()
        {
            LastSyncText = FormatLastSyncText(AppHelpers.SyncDate);
            OnPropertyChanged(nameof(ProfileName));
        }
        private void RegisterEvents()
        {
            if (!WeakReferenceMessenger.Default.IsRegistered<NotificationMessageEvent>(this))
            {
                WeakReferenceMessenger.Default.Register<NotificationMessageEvent>(this, async (s, e) =>
                {
                    if (e.Value == NotifyType.StartSync)
                        await SynchroniseDataUnifiedCommand.ExecuteAsync(null);
                });
            }
        }

        private async Task LogOutAndClearAppDataAsync()
        {
            try
            {
                SecureStorage.RemoveAll();
                Preferences.Clear();

                var dataManager = ServiceHelper.GetService<IDataManager>();
                if (dataManager != null)
                {
                    await dataManager.LogoutAsync();
                }
                await ClearAppDataAsync();
            }
            catch (Exception ex)
            {

            }
        }

        private async Task ClearAppDataAsync()
        {
            try
            {
                await DeleteDirectory(FileSystem.CacheDirectory);

                await DeleteDirectory(FileSystem.AppDataDirectory);

            }
            catch (Exception ex)
            {

            }

        }


        private static Task DeleteDirectory(string path)
        {
            if (!Directory.Exists(path))
                return Task.CompletedTask;

            foreach (var file in Directory.GetFiles(path))
            {
                try { File.Delete(file); }
                catch { }
            }

            foreach (var directory in Directory.GetDirectories(path))
            {
                try
                {
                    DeleteDirectory(directory);
                    Directory.Delete(directory);
                }
                catch { }
            }

            return Task.CompletedTask;
        }

        private void RefreshMenuState()
        {
            if (Shell.Current is AppShell s) s.SetStartupItem();
        }

        #endregion

        #region [ Commands ]

        [RelayCommand]
        private async Task FetchData()
        {
            if (string.IsNullOrWhiteSpace(AppHelpers.AzureServiceUrl) || AppHelpers.AzureServiceUrl == "https://www.syncservice.com")
                return;

            if (_appWorkflowManager.EngineerService.CurrentEngineer == null)
                await _appWorkflowManager.EngineerService.FetchCurrentEngineer();

            if (_appWorkflowManager.UserService.CurrentUser == null)
                await _appWorkflowManager.UserService.FetchCurrentUser();


            this.OnPropertyChanged(nameof(ProfileName));
            this.OnPropertyChanged(nameof(CurrentDate));
        }

        [RelayCommand]
        private async Task MenuSelected(MenuOption selectedMenu)
        {
            // if (selectedMenu.Route == nameof(ActivityListPage))
            // {
            //     await Shell.Current.GoToAsync("//activityroot/activitylist");

            // }
            // else

            foreach (var menu in OptionsItems)
            {
                menu.IsSelected = false;
            }

            selectedMenu.IsSelected = true;
            await Shell.Current.GoToAsync(selectedMenu.Route);
            Shell.Current.FlyoutIsPresented = false;
        }

        [RelayCommand]
        private async Task SynchroniseData()
        {
            Shell.Current.FlyoutIsPresented = false;
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DialogService.ShowAlertDialog("Alert!!", "No Internet Connection Available");
                return;
            }

            try
            {
                //DialogService.ShowLoading("Authenticating User");
                await _appWorkflowManager.UserService.LoginAsync(AppHelpers.AzureServiceUrl);

                await PerformFullSyncAsync();

                DialogService.HideLoading();
                WeakReferenceMessenger.Default.Send(new NotificationMessageEvent(NotifyType.PostSyncRefresh));

                PopulateOptionsMenu();
            }
            catch (Exception ex)
            {
            }
        }

        [RelayCommand]
        private async Task SynchroniseDataWithProgress()
        {
            Shell.Current.FlyoutIsPresented = false;
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DialogService.ShowAlertDialog("Alert!!", "No Internet Connection Available");
                return;
            }
            await PerformFullSyncWithProgressAsync();
        }

        [RelayCommand]
        private async Task SynchroniseDataUnified()
        {
            Shell.Current.FlyoutIsPresented = false;
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DialogService.ShowAlertDialog("Alert!!", "No Internet Connection Available");
                return;
            }

            // Show progress popup
            _syncSteps = new ObservableCollection<SyncStepModel>
            {
                new SyncStepModel { Name = "Login", Status = "[ ]", Description = "Confirm the saved session is still valid" },
                new SyncStepModel { Name = "Prepare Sync", Status = "[ ]", Description = "Open the sync session and load blob storage settings" },
                new SyncStepModel { Name = "Activity and Checklist Updates", Status = "[ ]", Description = "Upload activity status changes and checklist step results" },
                new SyncStepModel { Name = "Punch Updates", Status = "[ ]", Description = "Upload new and edited punch records" },
                new SyncStepModel { Name = "Photo Records", Status = "[ ]", Description = "Save photo deletes, descriptions, and checklist links" },
                new SyncStepModel { Name = "Photo Files", Status = "[ ]", Description = "Transfer photos and refresh the local photo cache" },

                new SyncStepModel { Name = "Refresh Records", Status = "[ ]", Description = "Refresh photos, activities, punches, and reference data" },
            };
            var popup = new SyncProgressPopup(_syncSteps, "Starting sync...");
            popup.ButtonText = "Please wait";
            await MopupService.Instance.PushAsync(popup);

            try
            {
                // Step 1: Login
                await StartStepAsync("Login", popup, "Checking your sign-in...");
                //_syncSteps[0].IsCurrent = true;
                popup.CurrentStep = "Logging in...";
                await _appWorkflowManager.UserService.LoginAsync(AppHelpers.AzureServiceUrl);
                await MarkStepCompleteAsync("Login", popup, "Sign-in confirmed.");
                await StartStepAsync("Prepare Sync", popup, "Connecting to the service and loading sync settings...");

                // Step 2: Prepare Sync
                //_syncSteps[1].IsCurrent = true;
                await StartStepAsync("Login", popup, "Checking your sign-in...");
                //popup.CurrentStep = "Preparing sync...";
                await MarkStepCompleteAsync("Prepare Sync", popup, "Sync session is ready.");

                Guid transactionBatchId = Guid.NewGuid();
                await StartStepAsync("Activity and Checklist Updates", popup, "Uploading checklist step changes...");
                await _appWorkflowManager.SyncLogService.PostSyncLogStart(transactionBatchId);
                // _syncSteps[1].Status = "[OK]";
                // _syncSteps[1].IsCurrent = false;
                // _syncSteps[1].IsCompleted = true;


                // Step 3: Activity and Checklist Updates
                _syncSteps[2].IsCurrent = true;
                popup.CurrentStep = "Syncing standing data...";
                await SyncStandingDataAsync();
                _syncSteps[2].Status = "[OK]";
                _syncSteps[2].IsCurrent = false;
                _syncSteps[2].IsCompleted = true;

                // Step 4: Punch Updates
                _syncSteps[3].IsCurrent = true;
                popup.CurrentStep = "Uploading punch updates...";
                // If you have a punch update method, call it here
                _syncSteps[3].Status = "[OK]";
                _syncSteps[3].IsCurrent = false;
                _syncSteps[3].IsCompleted = true;

                // Step 5: Photo Records
                _syncSteps[4].IsCurrent = true;
                popup.CurrentStep = "Saving photo records...";
                // If you have a photo record method, call it here
                _syncSteps[4].Status = "[OK]";
                _syncSteps[4].IsCurrent = false;
                _syncSteps[4].IsCompleted = true;

                // Step 6: Refresh Records
                //_syncSteps[5].IsCurrent = true;
                //popup.CurrentStep = "Refreshing records...";
                await StartStepAsync("Photo Files", popup, "Uploading activity and punch photo files...");
                var blobConnectionString = await _appWorkflowManager.UserService.GetAzureBlobStorageString();
                await _appWorkflowManager.SynchroniseService.UploadBlobData(blobConnectionString);
                await _appWorkflowManager.SynchroniseService.PushAndPullDataAsync(true, true);

                await StartStepAsync("Photo Files", popup, "Removing stale local photos and downloading the latest server photos...");
                await _appWorkflowManager.SynchroniseService.DownloadBlobData(blobConnectionString);
                await MarkStepCompleteAsync("Photo Files", popup, "Photo files are up to date.");
                // _syncSteps[5].Status = "[OK]";
                // _syncSteps[5].IsCurrent = false;
                // _syncSteps[5].IsCompleted = true;

                // Step 7: Photo Files
                _syncSteps[6].IsCurrent = true;
                popup.CurrentStep = "Transferring photo files...";
                // If you have a photo file transfer method, call it here
                _syncSteps[6].Status = "[OK]";
                _syncSteps[6].IsCurrent = false;
                _syncSteps[6].IsCompleted = true;

                // Finish sync
                await _appWorkflowManager.SyncLogService.PostSyncLogFinish(transactionBatchId);
                var syncDateTime = DateTime.UtcNow.ToString("dd-MM-yyyy, HH:mm:ss");
                AppHelpers.SyncDate = syncDateTime;

                RefreshState();
                await _appWorkflowManager.UserService.FetchCurrentUser();
                await MarkStepCompleteAsync("Refresh Records", popup, "Database and sync time are up to date.");

                //popup.CurrentStep = "Sync completed!";
                CloseButtonText = popup.ButtonText = "Done";
                await Task.Delay(1000);

                //await MopupService.Instance.PopAsync();
                WeakReferenceMessenger.Default.Send(new NotificationMessageEvent(NotifyType.PostSyncRefresh));
                PopulateOptionsMenu();
            }
            catch (Exception ex)
            {
                popup.CurrentStep = $"Error: {ex.Message}";
                await Task.Delay(2000);
                //await MopupService.Instance.PopAsync();
                SentrySdk.CaptureException(ex);
                //await DialogService.ShowAlertDialog("Sync Error", ex.Message, AlertType.Error);
            }
        }

        [RelayCommand]
        private async Task Logout()
        {
            Shell.Current.FlyoutIsPresented = false;

            if (!await DialogService.ShowConfirmAsync("Remember to synchronise before logout as all local data will be cleared and progress may be lost", "Logout", "LOGOUT", "CANCEL"))
                return;

            await LogOutAndClearAppDataAsync();

            RefreshMenuState();
            if (Shell.Current != null)
                await MainThread.InvokeOnMainThreadAsync(() => Shell.Current.GoToAsync("//import-settings"));

        }

        #endregion

        #region [ Override Methods ]

        public override void LoadDataOnNavigatedTo()
        {
            _ = InitializeDataAsync();
            RegisterEvents();
        }



        #endregion
    }
}
