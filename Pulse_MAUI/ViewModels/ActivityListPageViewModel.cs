using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pulse_MAUI.Constants;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models;
using Pulse_MAUI.Views;

namespace Pulse_MAUI.ViewModels
{
    public partial class ActivityListPageViewModel : BaseListViewModel
    {
        #region [ Properties ]

        readonly IActivityService _activityService;
        readonly IActivitySearchService _activitySearchService;

        [ObservableProperty]
        private ObservableCollection<Activity> _activities = new();

        public List<Activity>? tempActivity { get; private set; }


        #endregion

        public ActivityListPageViewModel(IActivityService activityService,
            IActivitySearchService activitySearchService,
            IViewModelParameters viewModelParameters) : base(viewModelParameters)
        {
            _activityService = activityService;
            _activitySearchService = activitySearchService;
        }

        #region [ Methods & Service Calls ]

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

        private async Task InitializeDataAsync()
        {
            await RefreshCommand.ExecuteAsync(null);
        }

        #endregion

        #region [ Commands ]


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

        public override void LoadDataOnAppearing()
        {
            _ = InitializeDataAsync();
        }

        protected override void OnUnitChanged(string? newValue)
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

        protected override void OnCommSystemChanged(string? newValue)
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

        protected override void OnComponentTagChanged(string? newValue)
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

        protected override void OnComponentTypeChanged(string? newValue)
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

        protected override void OnActivityChanged(string? newValue)
        {
            FilterResults();
        }

        protected override async Task RefereshItems()
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

        #endregion
    }
}
