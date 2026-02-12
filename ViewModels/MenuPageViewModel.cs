using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Pulse_MAUI.Enums;
using Pulse_MAUI.Events;
using Pulse_MAUI.Helpers;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models;
using Pulse_MAUI.Resources.Languages;
using Pulse_MAUI.Services;
using Pulse_MAUI.Views;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

namespace Pulse_MAUI.ViewModels
{
    public partial class MenuPageViewModel : BaseViewModel
    {
        #region [ Properties ]

        readonly IEngineerService _engineerService;

        [ObservableProperty]
        ObservableCollection<MenuOption>? _optionsItems;

        [ObservableProperty]
        private string? _currenDate = DateTime.Now.ToString("dd MMMM yyyy HH:mm:ss");

        public string ProfileName
        {
            get
            {
                return string.Format(UserInterface.MenuPage_HelloTitle,
                                     _engineerService.CurrentEngineer != null ?
                                     _engineerService.CurrentEngineer.Name :
                                     "N/A");
            }

        }
        #endregion
        public MenuPageViewModel(IDialogService dialogService,IEngineerService engineerService) : base(dialogService)
        {
            _engineerService = engineerService;
            PopulateOptionsMenu();

            App.Current?.Dispatcher.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                this.OnPropertyChanged("CurrentDate");
                return true;
            });
        }

        #region [ Methods && Service Calls ]

        /// <summary>
		/// Populate the items for the options menu.
		/// </summary>
		private void PopulateOptionsMenu()
        {
            if (Keys.AzureService == "https://www.syncservice.com")
            {
                OptionsItems = new ObservableCollection<MenuOption>
                {
                    new MenuOption{
                    Title = "Import Settings",
                    TargetType = typeof(ImportSettingsPage),
                    Index = 2
                    }
                };
            }
            else
            {
                OptionsItems = new ObservableCollection<MenuOption>
                {
                  new MenuOption{
                      Title = "Activities",
                      TargetType = typeof(ActivityListPage),
                      Index = 0
                  }
                  ,new MenuOption{
                      Title = "Punch List",
                      TargetType = typeof(PunchListPage),
                      Index = 1
                  }
                };
            }


        }

        #endregion

        #region [ Commands ]

        [RelayCommand]
        private async Task FetchData()
        {
            if (_engineerService.CurrentEngineer == null)
                await _engineerService.FetchCurrentEngineer();

            if (UserService.Instance.CurrentUser == null)
                await UserService.Instance.FetchCurrentUser();

            
            this.OnPropertyChanged(nameof(ProfileName));
            this.OnPropertyChanged(nameof(CurrenDate));
        }

        [RelayCommand]
        private async Task SynchroniseData()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DialogService.ShowAlertDialog("No Internet Connection Available",false);
                return;
            }

            DialogService.ShowLoading();
            await UserService.Instance.LoginAsync();

            //if (UserService.Instance.AADAuthenticated == true && SynchroniseService.Instance.isUserValid() == true)
            //{

            //    // Create and display the value of two GUIDs.
            //    Guid TransactionBatchId;
            //    TransactionBatchId = Guid.NewGuid();

            //    await SyncLogService.Instance.PostSyncLogStart(TransactionBatchId);

            //    var connectionString = await UserService.Instance.GetAzureBlobStorageString();

            //    if (CrossConnectivity.Current.IsConnected)
            //    {

            //        List<string> result = new List<string>();
            //        // Service failure handling here!
            //        try
            //        {
            //            //Do a full or incremental pull of the standing data
            //            IEnumerable<Project> projectData = await LookupService.Instance.GetProjectListAsync();

            //            if (projectData != null && projectData.Count() > 0)
            //            {
            //                DialogService.ShowLoading(UserInterface.MenuPage_Synchronising);
            //                result = await SynchroniseService.Instance.PushAndPullDataAsync(true, false);
            //            }
            //            else
            //            {
            //                DialogService.ShowLoading(UserInterface.MenuPage_Synchronising + " (Full)");
            //                result = await SynchroniseService.Instance.PushAndPullDataAsync(false, false);
            //            }

            //        }
            //        catch (MobileServiceInvalidOperationException ex)
            //        {
            //            Debug.WriteLine(ex.Message);
            //        }

            //        catch (Exception ex)
            //        {
            //            Debug.WriteLine(ex.Message);
            //            await DialogService.DisplayAlertAsync("Unable to complete data sync", "Sync Error");
            //        }


            //        if (result.Count > 0)
            //        {
            //            StringBuilder sb = new StringBuilder();
            //            foreach (String err in result)
            //            {
            //                sb.AppendLine(err);
            //            }

            //            await DialogService.DisplayAlertAsync(sb.ToString(), "Sync Error");
            //        }


            //    }
            //    else
            //    {
            //        DialogService.ShowError("No Internet Connection Available");
            //        return;
            //    }


            //    if (CrossConnectivity.Current.IsConnected)
            //    {
            //        DialogService.ShowLoading("Uploading Image Items");
            //        await SynchroniseService.Instance.UploadBlobData(connectionString);
            //    }
            //    else
            //    {
            //        DialogService.ShowError("No Internet Connection Available");
            //        return;
            //    }

            //    if (CrossConnectivity.Current.IsConnected)
            //    {
            //        DialogService.ShowLoading(UserInterface.MenuPage_Synchronising);
            //        await SynchroniseService.Instance.PushAndPullDataAsync(true, true);
            //    }
            //    else
            //    {
            //        DialogService.ShowError("No Internet Connection Available");
            //        return;
            //    }


            //    if (CrossConnectivity.Current.IsConnected)
            //    {
            //        DialogService.ShowLoading("Downloading Image Items");
            //        await SynchroniseService.Instance.DownloadBlobData(connectionString);
            //    }
            //    else
            //    {
            //        DialogService.ShowError("No Internet Connection Available");
            //        return;
            //    }

            //    await SyncLogService.Instance.PostSyncLogFinish(TransactionBatchId);
            //    // store the last sync date
            //    AppSettings.AddOrUpdateValue("SyncDate", DateTime.UtcNow.ToString("dd-MM-yyyy, HH:mm:ss"));


            //    await UserService.Instance.FetchCurrentUser();
            //    var currentUser = UserService.Instance.CurrentUser;

            //    if (currentUser != null)
            //    {

            //        try
            //        {

            //            await Task.Delay(2000);
            //            // implement the required lists
            //            await ActivityService.Instance.FetchActivityListAsync();
            //            await PunchService.Instance.FetchPunchListAsync();
            //            await UserService.Instance.FetchCurrentUser();
            //            await EngineerService.Instance.FetchCurrentEngineer();
            //            await Task.Delay(2000);

            //            await ActivitySearchService.Instance.FetchSearchItems();
            //            await PunchSearchService.Instance.FetchSearchItems();

            //            this.OnPropertyChanged("ProfileName");
            //            this.OnPropertyChanged("CurrentDate");
            //        }

            //        catch (Exception ex)
            //        {
            //            await DialogService.DisplayAlertAsync(ex.Message, "Sync Error");
            //        }
            //    }
            //    else
            //    {
            //        // Display warning and force user logout
            //        await DialogService.DisplayAlertAsync("You are not a registered user", "Warning");
            //        await UserService.Instance.LogoutAsync();
            //    }

            //    if (!System.Diagnostics.Debugger.IsAttached)
            //    {
            //        //if (CrossConnectivity.Current.IsConnected)
            //        // {
            //        //   await UserService.Instance.LogoutAsync();
            //        // }
            //    }



            //}
            //else
            //{
            //    // invalid user
            //    await DialogService.ShowAlertDialog("Invalid User Login",false);
            //}

            DialogService.HideLoading();
            WeakReferenceMessenger.Default.Send(new NotificationMessageEvent(NotifyType.PostSyncRefresh));
        }

        #endregion
    }
}
