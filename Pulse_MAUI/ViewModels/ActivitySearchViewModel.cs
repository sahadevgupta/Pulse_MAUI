using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Pulse_MAUI.Events;
using Pulse_MAUI.Interfaces;
using System.Collections.ObjectModel;

namespace Pulse_MAUI.ViewModels
{
    public partial class ActivitySearchViewModel : BaseViewModel
    {
        #region [ Properties ]
        readonly IActivitySearchService _activitySearchService;

        [ObservableProperty]
        private ObservableCollection<string>? _activitiesName;

        [ObservableProperty]
        private ObservableCollection<string>? _componentTypes;

        [ObservableProperty]
        private ObservableCollection<string>? _componentTags;

        [ObservableProperty]
        private ObservableCollection<string>? _units;

        [ObservableProperty]
        private ObservableCollection<string>? _commSystem;

        [ObservableProperty]
        private string? _selectedActivity;

        [ObservableProperty]
        private string? _selectedUnit;

        [ObservableProperty]
        private string? _selectedCommSystem;

        [ObservableProperty]
        private string? _selectedComponentType;

        [ObservableProperty]
        private string? _selectedComponentTag;


        #endregion

        public ActivitySearchViewModel(IActivitySearchService activitySearchService, IViewModelParameters viewModelParameters) : base(viewModelParameters)
        {
            _activitySearchService = activitySearchService;
            InitializeData();
        }

        #region [ Methods & Service Calls ]

        private void InitializeData()
        {
            ActivitiesName = _activitySearchService.ActivitiesName;
            CommSystem = _activitySearchService.CommSystem;
            ComponentTags = _activitySearchService.ComponentTags;
            ComponentTypes = _activitySearchService.ComponentTypes;
            Units = _activitySearchService.Units;

        }

        partial void OnSelectedActivityChanged(string? value)
        {
            FilterResults();
        }

        partial void OnSelectedUnitChanged(string? value)
        {
            if (!(value == null || value == "All"))
            {
                // rebind the Comm system with the filtered selection
                CommSystem = _activitySearchService.FetchCommSystemByUnit(value);
                this.OnPropertyChanged("SelectedCommSystem");
            }
            else
            {
                CommSystem = _activitySearchService.FetchCommSystem();
                this.OnPropertyChanged("SelectedCommSystem");
            }
            FilterResults();
        }

        partial void OnSelectedCommSystemChanged(string? value)
        {
            if (!(value == null || value == "All"))
            {
                ComponentTypes = _activitySearchService.FetchComponentTypesByCommSystem(value);
                this.OnPropertyChanged("SelectedComponentType");
            }
            else
            {
                ComponentTypes = _activitySearchService.FetchComponentTypes();
                this.OnPropertyChanged("SelectedComponentType");
            }
            FilterResults();
        }

        partial void OnSelectedComponentTypeChanged(string? value)
        {
            if (!(value == null || value == "All"))
            {
                ComponentTags = _activitySearchService.FetchComponentTagsByCompType(value);
                this.OnPropertyChanged("SelectedComponentTag");
            }
            else
            {
                ComponentTags = _activitySearchService.FetchComponentTags();
                this.OnPropertyChanged("SelectedComponentTag");
            }
            FilterResults();
        }

        partial void OnSelectedComponentTagChanged(string? value)
        {
            if (!(value == null || value == "All"))
            {
                ActivitiesName = _activitySearchService.FetchActivitiesByComponentTag(value);
                this.OnPropertyChanged("SelectedActivity");
            }
            else
            {
               
                ActivitiesName = _activitySearchService.FetchActivities();
                this.OnPropertyChanged("SelectedActivity");
            }
            FilterResults();
        }

        private void FilterResults()
        {
            WeakReferenceMessenger.Default.Send(new NotificationMessageEvent(Enums.NotifyType.ActivityFilterRefresh));
        }

        #endregion
    }
}
