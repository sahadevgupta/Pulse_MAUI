using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Pulse_MAUI.Constants;
using Pulse_MAUI.Events;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models;
using Pulse_MAUI.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Pulse_MAUI.ViewModels
{
    public partial class ActivityListPageViewModel : BaseViewModel
    {
        #region [ Properties ]

        readonly IActivityService _activityService;

        [ObservableProperty]
        private ObservableCollection<Activity>? _activities;

        #endregion

        public ActivityListPageViewModel(IActivityService activityService, IViewModelParameters viewModelParameters) : base(viewModelParameters)
        {
            _activityService = activityService;
        }

        #region [ Methods & Service Calls ]

        public async Task InitializeDataAsync()
        {
            await RefreshActivityListCommand.ExecuteAsync(null);
        }

        public async Task RegisterEvents()
        {
            WeakReferenceMessenger.Default.Register<NotificationMessageEvent>(this, async (r, m) => await OnNotificationMessageReceived());
        }

        public void DeregisterEvents()
        {
            WeakReferenceMessenger.Default.Unregister<NotificationMessageEvent>(this);
        }

        private async Task OnNotificationMessageReceived()
        {
            await RefreshActivityListCommand.ExecuteAsync(null);
        }

        #endregion

        #region [ Commands ]

        [RelayCommand]
        private async Task RefreshActivityList()
        {
            var result = await _activityService.FetchFilteredActivitiesList();
            Activities = new ObservableCollection<Activity>(result);
        }

        [RelayCommand]
        private async Task ViewActivity(Activity selectedActivity)
        {
            var param = new Dictionary<string, object>
            {
                { NavigationParamConstant.Activity, selectedActivity }
            };
            await NavigationService.NavigateToPage<ActivityPage>(parameters: param);
        }

        #endregion

        #region [ Override Methods ]

        public override async Task LoadDataOnAppearing()
        {
            await InitializeDataAsync();
        }

        public override async Task LoadDataOnNavigatedTo()
        {
            await RegisterEvents();
        }

        public override async Task LoadDataOnDisappearing()
        {
            DeregisterEvents();
            await Task.CompletedTask;
        }

        #endregion
    }
}
