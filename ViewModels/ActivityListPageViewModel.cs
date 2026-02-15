using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models;
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

            RefreshActivityListCommand.Execute(null);
        }

        #region [ Commands ]

        [RelayCommand]
        private async Task RefreshActivityList()
        {
            var result = await _activityService.FetchFilteredActivitiesList();
            Activities = new ObservableCollection<Activity>(result);
        }
        #endregion
    }
}
