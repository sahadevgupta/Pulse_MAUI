using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pulse_MAUI.Constants;
using Pulse_MAUI.Helpers;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models;
using Pulse_MAUI.Resources.Languages;
using Pulse_MAUI.Services;

namespace Pulse_MAUI.ViewModels;

[QueryProperty(nameof(Activity), NavigationParamConstant.Activity)]
public partial class PunchPageViewModel(IViewModelParameters viewModelParameters,
    IDisciplineService disciplineService,
    IPunchService punchService,
    IPriorityService priorityService,
    ILookupService lookupService) : BaseViewModel(viewModelParameters)
{
    #region [ Properties ]

    IEnumerable<Project> availableProjects = Enumerable.Empty<Project>();
    IEnumerable<Lookup> availableStatusList = Enumerable.Empty<Lookup>();
    bool newPunch;
    int OpenStatusId;

    [ObservableProperty]
    private Activity? _activity;

    [ObservableProperty]
    private ObservableCollection<string> _priorityList = new();

    [ObservableProperty]
    private ObservableRangeCollection<string> _projects = new();

    [ObservableProperty]
    private ObservableRangeCollection<string> _statusList = new();

    [ObservableProperty]
    private ObservableRangeCollection<string> _disciplineList = new();

    [ObservableProperty]
    private bool _createdFromActivityItem;

    [ObservableProperty]
    private string? _selectedPriority;

    [ObservableProperty]
    private string? _selectedProject;

    [ObservableProperty]
    private string? _selectedDiscipline;

    [ObservableProperty]
    private string? _selectedStatus;

    [ObservableProperty]
    private int _activityControlType;

    [ObservableProperty]
    private PunchItem? _punch;

    #endregion

    #region [ Methods & Service Calls ]

    private async Task InitializeDataAsync()
    {
        Punch = await punchService.CreatePunchItem();
        //Set some default values
        Punch.Controltype = ActivityControlType;

        CreatedFromActivityItem = true;

        Punch.PCAId = Activity?.PCAId;

        Punch.PCCId = Activity?.PCCId;
        Punch.ProjectId = Activity!.ProjectId.GetValueOrDefault();
        Punch.TagId = Activity.TagId;
        Punch.Description = "";
        Punch.CreatedOn = DateTime.UtcNow;

        Punch.PUId = Activity.PUId;
        Punch.PUCId = Activity.PUCId;
        Punch.ComponentType = Activity.ComponentType;
        Punch.AssignedEngineerDate = DateTime.UtcNow;

        Punch.Status = OpenStatusId;
    }

    /// <summary>
    /// Populates the lists.
    /// </summary>
    private async Task PopulateListsAsync()
    {
        if (Projects.Count == 0)
            await PopulateProjectsCommand.ExecuteAsync(null);

        if (StatusList.Count == 0)
            await ExecuteFetchStatusListCommand.ExecuteAsync(null);

        await ExecutePopulateDisciplineListCommand.ExecuteAsync(null);
    }

    #endregion

    #region [ Commands ]

    [RelayCommand]
    public async Task PopulateProjects()
    {
        Projects.Clear();

        availableProjects = await lookupService.GetProjectListAsync();

        if (availableProjects != null && availableProjects.Count() > 0)
        {
            var availableProjectNames = availableProjects
                .OrderBy(p => p.Name)
                .Select(p => p.Name);

            Projects.AddRange(availableProjectNames, System.Collections.Specialized.NotifyCollectionChangedAction.Add);

            if (Punch == null)
            {
                Punch = await punchService.CreatePunchItem();
            }


            if (Punch.ProjectId == 0)
            {
                Punch.ProjectId = availableProjects.FirstOrDefault().ProjectId;
            }

            SelectedProject = availableProjects
                    .FirstOrDefault(p => p.ProjectId == Punch.ProjectId)
                    .Name;

            // if we have a valid project Id then we need to populate the priority list
            if (Punch.ProjectId != 0)
            {
                PopulatePriorityListCommand.ExecuteAsync(Punch.ProjectId);
            }

        }
    }

    [RelayCommand]
    public async Task PopulatePriorityList(int projectId)
    {

        if (projectId != 0)
        {
            PriorityList.Clear();

            var priorityItems = await priorityService.GetPriorityListForProjectAsync(projectId);

            var priorityItemsText = priorityItems
                .Select(p => p.Value)
                   .Distinct();


            PriorityList = new ObservableCollection<string>(priorityItemsText);

            Priority? selected = priorityItems.FirstOrDefault(p => p.PriorityId == Punch.Priority);
            if (selected != null)
            {
                SelectedPriority = selected.Value;
            }
            else
            {

                if (priorityItems.Count() > 0)
                {

                    int defaultindex = priorityItems.Min(p => p.PriorityId);
                    Priority defaultval = priorityItems.Where(p => p.PriorityId == defaultindex).FirstOrDefault();
                    SelectedPriority = defaultval.Value;
                }
            }
        }
    }

    [RelayCommand]
    private async Task ExecuteFetchStatusList()
    {

        StatusList.Clear();

        availableStatusList = await lookupService.GetStatusLookups();

        //StatusList.Add(UserInterface.PunchPage_PickerPlaceHolder);

        if (availableStatusList != null && availableStatusList.Count() > 0)
        {
            foreach (var availableStatus in availableStatusList)
            {
                StatusList.Add(availableStatus.Value);
            }

            if (Punch == null)
            {
                Punch = await punchService.CreatePunchItem();
            }

            var status = availableStatusList
                .FirstOrDefault(p => p.LookupId == Punch.Status);

            OpenStatusId = await lookupService.GetStatusLookupId("Open");

            // New punch items can only be set as open
            if (newPunch)
            {
                var ClosedStatusId = await lookupService.GetStatusLookupId("Closed");
                status = availableStatusList
                .FirstOrDefault(p => p.LookupId == OpenStatusId);
            }



            SelectedStatus = status != null ? status.Value : UserInterface.PunchPage_PickerPlaceHolder;
        }
    }

    [RelayCommand]
    private async Task ExecutePopulateDisciplineList()
    {
        string selectedDisciplineLocal = "";
        ObservableRangeCollection<string> disciplineListLocal = new ObservableRangeCollection<string>();

        disciplineListLocal.Clear();
        var disciplineItems = await disciplineService.GetDisciplineListAsync();
        var disciplineItemsText = disciplineItems
                .Select(p => p.Name)
                    .Distinct();

        disciplineListLocal.AddRange(disciplineItemsText, System.Collections.Specialized.NotifyCollectionChangedAction.Add);

        await Task.Run(() =>
        {

            Discipline? selected = disciplineItems.FirstOrDefault(p => p.DisciplineId == Punch.DisciplineId);
            if (selected != null)
            {
                selectedDisciplineLocal = selected.Name;
            }
        });

        DisciplineList = disciplineListLocal;
        SelectedDiscipline = selectedDisciplineLocal;

    }

    #endregion

    #region [ Override Methods ]

    public override Task LoadDataOnNavigatedTo()
    {
        return base.LoadDataOnNavigatedTo();
    }

    #endregion

}
