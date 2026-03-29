using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Controls;
using Pulse_MAUI.Constants;
using Pulse_MAUI.Events;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models;
using Pulse_MAUI.Resources.Languages;
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
        readonly IActivitySearchService _activitySearchService;

        [ObservableProperty]
        private ObservableCollection<Activity> _activities = new();

        [ObservableProperty]
        private ObservableCollection<string>? _commSystem;

        [ObservableProperty]
        private ObservableCollection<string>? _componentTags;

        [ObservableProperty]
        private ObservableCollection<string>? _componentTypes;

        [ObservableProperty]
        private ObservableCollection<string>? _units;

        [ObservableProperty]
        private ObservableCollection<string>? _activitiesName;

        [ObservableProperty]
        private string? _selectedUnit;

        [ObservableProperty]
        private string? _selectedCommSystem;

        [ObservableProperty]
        private string? _selectedComponentType;

        [ObservableProperty]
        private string? _selectedComponentTag;

        [ObservableProperty]
        private string? _selectedActivity;

        public List<Activity> tempActivity { get; private set; }


        #endregion

        public ActivityListPageViewModel(IActivityService activityService,
            IActivitySearchService activitySearchService,
            IViewModelParameters viewModelParameters) : base(viewModelParameters)
        {
            _activityService = activityService;
            _activitySearchService = activitySearchService;
        }

        #region [ Methods & Service Calls ]

        partial void OnSelectedUnitChanged(string? oldValue, string? newValue)
        {
            if (!(newValue == null || newValue == "All"))
            {
                // rebind the Comm system with the filtered selection
                CommSystem = _activitySearchService.FetchCommSystemByUnit(newValue);
                this.OnPropertyChanged("SelectedCommSystem");
            }
            else
            {
                CommSystem = _activitySearchService.FetchCommSystem();
                this.OnPropertyChanged("SelectedCommSystem");
            }
            FilterResults();
        }

        partial void OnSelectedCommSystemChanged(string? oldValue, string? newValue)
        {
            if (!(newValue == null || newValue == "All"))
            {
                ComponentTypes = _activitySearchService.FetchComponentTypesByCommSystem(newValue);
                this.OnPropertyChanged("SelectedComponentType");
            }
            else
            {
                ComponentTypes = _activitySearchService.FetchComponentTypes();
                this.OnPropertyChanged("SelectedComponentType");
            }

            FilterResults();
        }

        partial void OnSelectedComponentTypeChanged(string? oldValue, string? newValue)
        {
            if (!(newValue == null || newValue == "All"))
            {
                ComponentTags = _activitySearchService.FetchComponentTagsByCompType(newValue);
                this.OnPropertyChanged("SelectedComponentTag");
            }
            else
            {
                ComponentTags = _activitySearchService.FetchComponentTags();
                this.OnPropertyChanged("SelectedComponentTag");
            }

            this.OnPropertyChanged("SelectedComponentType");
            FilterResults();
        }

        partial void OnSelectedComponentTagChanged(string? oldValue, string? newValue)
        {
            if (!(newValue == null || newValue == "All"))
            {
                ActivitiesName = _activitySearchService.FetchActivitiesByComponentTag(newValue);
                this.OnPropertyChanged("SelectedActivity");
            }
            else
            {
                ActivitiesName = _activitySearchService.FetchActivities();
                this.OnPropertyChanged("SelectedActivity");
            }


            this.OnPropertyChanged("SelectedComponentTag");
            FilterResults();
        }

        partial void OnSelectedActivityChanged(string? oldValue, string? newValue)
        {
            FilterResults();
        }

        private void FilterResults()
        {
            var availableActivities = new List<Activity>(tempActivity);

            if (HasOptionSelected(SelectedUnit))
            {
                availableActivities = availableActivities
                                        .Where(p => p.Unit == SelectedUnit)
                                        .ToList();
            }

            if (HasOptionSelected(SelectedCommSystem))
            {
                availableActivities = availableActivities
                                        .Where(p => p.CommissioningSystem == SelectedCommSystem)
                                        .ToList();
            }

            if (HasOptionSelected(SelectedComponentType))
            {
                availableActivities = availableActivities
                                        .Where(p => p.ComponentType == SelectedComponentType)
                                        .ToList();
            }

            if (HasOptionSelected(SelectedComponentTag))
            {
                availableActivities = availableActivities
                                        .Where(p => p.TagId == SelectedComponentTag)
                                        .ToList();
            }

            if (HasOptionSelected(SelectedActivity))
            {
                availableActivities = availableActivities
                                        .Where(p => p.Name == SelectedActivity)
                                        .ToList();
            }
            Activities = new ObservableCollection<Activity>(availableActivities);
        }


        public bool HasOptionSelected(string? option)
        {
            if (!string.IsNullOrEmpty(option) && option != UserInterface.SearchView_All)
            {
                return true;
            }

            return false;
        }

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
            tempActivity = new List<Activity>(result);

            Units = new ObservableCollection<string>(_activitySearchService.Units ?? Enumerable.Empty<string>());
            CommSystem = new ObservableCollection<string>(_activitySearchService.CommSystem ?? Enumerable.Empty<string>());
            ComponentTags = new ObservableCollection<string>(_activitySearchService.ComponentTags ?? Enumerable.Empty<string>());
            ComponentTypes = new ObservableCollection<string>(_activitySearchService.ComponentTypes ?? Enumerable.Empty<string>());
            ActivitiesName = new ObservableCollection<string>(_activitySearchService.ActivitiesName ?? Enumerable.Empty<string>());

        }

        [RelayCommand]
        private async Task ViewActivity(Activity selectedActivity)
        {
            var param = new Dictionary<string, object>
            {
                { NavigationParamConstant.Activity, selectedActivity }
            };
            await NavigationService.NavigateToPage<ActivityPage>(parameters: param);

            //await Shell.Current.GoToAsync($"//activityroot/activity", param);
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
