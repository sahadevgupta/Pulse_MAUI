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
    public partial class ActivityListPageViewModel(IActivityService activityService, IViewModelParameters viewModelParameters) : BaseViewModel(viewModelParameters)
    {
        #region [ Properties ]

        [ObservableProperty]
        private ObservableCollection<Activity>? _activities;

        #endregion

        #region [ Commands ]

        [RelayCommand]
        private async Task RefreshActivityList()
        {
            var result = await activityService.FetchFilteredActivitiesList();
            Activities = new ObservableCollection<Activity>(result);
        }
        #endregion
    }
}
