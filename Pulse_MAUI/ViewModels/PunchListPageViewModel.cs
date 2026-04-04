using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models;
using Pulse_MAUI.Views;

namespace Pulse_MAUI.ViewModels;

public partial class PunchListPageViewModel(IViewModelParameters viewModelParameters,
    IActivityService activityService,
    IPunchService punchService,
    IPunchSearchService punchSearchService) : BaseListViewModel(viewModelParameters)
{
    #region [ Properties ]

    [ObservableProperty]
    private ObservableCollection<PunchItem>? _punches;

    private List<PunchItem>? tempPunches { get; set; }

    #endregion


    #region [ Methods & Service Calls ]

    private async Task InitializeDataAsync()
    {
        await RefreshCommand.ExecuteAsync(null);
    }

    private async Task FilterResults()
    {
        var availablePunches = new List<PunchItem>(tempPunches ?? Enumerable.Empty<PunchItem>());

        if (HasOptionSelected(SelectedUnit))
        {
            availablePunches = availablePunches
                                    .Where(p => p.Unit == SelectedUnit)
                                    .ToList();
        }

        if (HasOptionSelected(SelectedCommSystem))
        {
            availablePunches = availablePunches
                                    .Where(p => p.CommissioningSystem == SelectedCommSystem)
                                    .ToList();
        }

        if (HasOptionSelected(SelectedComponentType))
        {
            availablePunches = availablePunches
                                    .Where(p => p.ComponentType == SelectedComponentType)
                                    .ToList();
        }

        if (HasOptionSelected(SelectedComponentTag))
        {
            availablePunches = availablePunches
                                    .Where(p => p.TagId == SelectedComponentTag)
                                    .ToList();
        }

        if (HasOptionSelected(SelectedActivity))
        {
            await activityService.FetchActivityListAsync();
            var activity = activityService
                    .Activities?
                    .FirstOrDefault(p => p.Name == SelectedActivity);

            if (activity != null)
            {
                availablePunches = availablePunches
                    .Where(p => p.PCAId == activity.PCAId)
                    .ToList();
            }

        }
        Punches = new ObservableCollection<PunchItem>(availablePunches);
    }

    #endregion

    #region [ Commands ]

    [RelayCommand]
    private async Task AddPunch()
    {
        await NavigationService.NavigateToPage<PunchPage>();
    }

    #endregion

    #region [ Override Methods ]

    public override async Task LoadDataOnAppearing()
    {
        await InitializeDataAsync();
    }

    protected override async void OnUnitChanged(string? newValue)
    {
        try
        {
            if (!(newValue == null || newValue == "All"))
            {
                // rebind the Comm system with the filtered selection
                CommSystem = punchSearchService.FetchCommSystemByUnit(newValue);
                this.OnPropertyChanged("SelectedCommSystem");
            }
            else
            {
                CommSystem = punchSearchService.FetchCommSystem();
                this.OnPropertyChanged("SelectedCommSystem");
            }
            await FilterResults();
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    protected override async void OnCommSystemChanged(string? newValue)
    {
        try
        {


            if (!(newValue == null || newValue == "All"))
            {
                ComponentTypes = punchSearchService.FetchComponentTypesByCommSystem(newValue);
                this.OnPropertyChanged("SelectedComponentType");
            }
            else
            {
                ComponentTypes = punchSearchService.FetchComponentTypes();
                this.OnPropertyChanged("SelectedComponentType");
            }

            await FilterResults();
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    protected override async void OnComponentTagChanged(string? newValue)
    {
        try
        {
            if (!(newValue == null || newValue == "All"))
            {
                ActivitiesName = punchSearchService.FetchActivitiesByComponentTag(newValue);
                this.OnPropertyChanged("SelectedActivity");
            }
            else
            {
                ActivitiesName = punchSearchService.FetchActivities();
                this.OnPropertyChanged("SelectedActivity");
            }


            this.OnPropertyChanged("SelectedComponentTag");
            await FilterResults();
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    protected override async void OnComponentTypeChanged(string? newValue)
    {
        try
        {


            if (!(newValue == null || newValue == "All"))
            {
                ComponentTags = punchSearchService.FetchComponentTagsByCompType(newValue);
                this.OnPropertyChanged("SelectedComponentTag");
            }
            else
            {
                ComponentTags = punchSearchService.FetchComponentTags();
                this.OnPropertyChanged("SelectedComponentTag");
            }

            this.OnPropertyChanged("SelectedComponentType");
            await FilterResults();
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }
    protected override async void OnActivityChanged(string? newValue)
    {
        await FilterResults();
    }

    protected override async Task RefereshItems()
    {
        await punchService.FetchPunchListAsync();
        Punches = new ObservableCollection<PunchItem>(punchService.Punches ?? Enumerable.Empty<PunchItem>());
        tempPunches = new List<PunchItem>(punchService.Punches ?? Enumerable.Empty<PunchItem>());

        Units = new ObservableCollection<string>(punchSearchService.Units ?? Enumerable.Empty<string>());
        CommSystem = new ObservableCollection<string>(punchSearchService.CommSystem ?? Enumerable.Empty<string>());
        ComponentTags = new ObservableCollection<string>(punchSearchService.ComponentTags ?? Enumerable.Empty<string>());
        ComponentTypes = new ObservableCollection<string>(punchSearchService.ComponentTypes ?? Enumerable.Empty<string>());
        ActivitiesName = new ObservableCollection<string>(punchSearchService.Activities ?? Enumerable.Empty<string>());
    }

    #endregion
}
