using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pulse_MAUI.Constants;
using Pulse_MAUI.Enums;
using Pulse_MAUI.Helpers;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models;
using Pulse_MAUI.Resources.Languages;
using Pulse_MAUI.Services;
using Pulse_MAUI.Views;

namespace Pulse_MAUI.ViewModels;

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
    private ObservableCollection<string>? _commSystems;

    [ObservableProperty]
    private ObservableCollection<string>? _componentTags;

    [ObservableProperty]
    private ObservableCollection<string>? _componentTypes;

    [ObservableProperty]
    private ObservableCollection<string>? _units;

    [ObservableProperty]
    private ObservableCollection<string>? _activitiesName;

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

    [ObservableProperty]
    private bool _isNewPunch;

    [ObservableProperty]
    private string? _idFormatted;

    private IEnumerable<Unit>? availableUnits;
    private IEnumerable<Component>? availableComponents;
    private IEnumerable<CommissioningSystem>? availableCommSystems;
    private IEnumerable<Activity>? availableActivities;

    #endregion

    #region [ Methods & Service Calls ]

    partial void OnSelectedProjectChanged(string? value)
    {
        Task.Run(async () =>
        {
            await FetchUnitsCommand.ExecuteAsync(null);
        });

    }

    partial void OnSelectedUnitChanged(string? value)
    {
        SelectedCommSystem = null;
        CommSystems?.Clear();

        SelectedComponentType = null;
        ComponentTypes?.Clear();

        SelectedComponentTag = null;
        ComponentTags?.Clear();

        SelectedActivity = null;
        ActivitiesName?.Clear();
        Task.Run(async () =>
        {
            await FetchCommSystemsCommand.ExecuteAsync(null);
        });
    }

    partial void OnSelectedCommSystemChanged(string? value)
    {
        SelectedComponentType = null;
        ComponentTypes?.Clear();

        SelectedComponentTag = null;
        ComponentTags?.Clear();

        SelectedActivity = null;
        ActivitiesName?.Clear();

        Task.Run(async () =>
        {
            await FetchComponentTypesCommand.ExecuteAsync(null);
        });
    }

    partial void OnSelectedComponentTypeChanged(string? value)
    {
        SelectedComponentTag = null;
        ComponentTags?.Clear();

        SelectedActivity = null;
        ActivitiesName?.Clear();

        Task.Run(async () =>
        {
            await FetchComponentTagsCommand.ExecuteAsync(null);
        });
    }

    partial void OnSelectedComponentTagChanged(string? value)
    {
        SelectedActivity = null;
        ActivitiesName?.Clear();

        Task.Run(async () =>
        {
            await FetchActivitiesCommand.ExecuteAsync(null);
        });
    }

    internal async Task OnBackPressed()
    {
        if (IsNewPunch)
        {
            var response = await Shell.Current.DisplayAlertAsync(
                UserInterface.PunchPage_PromptForSaveTitle,
                UserInterface.PromptAbandon,
                UserInterface.ConfirmYes,
                UserInterface.ConfirmNo
            );
            if (response)
            {
                await NavigationService.NavigateBack();
            }
        }
        else
        {
            await NavigationService.NavigateBack();
        }
    }


    private void InitializeData()
    {
        IdFormatted = string.Format(UserInterface.PunchPage_Id, Punch != null ? Punch.PunchId : 0);
        Task.Run(async () =>
        {
            if (IsNewPunch)
            {
                Punch = await punchService.CreatePunchItem();
                Punch.CreatedOn = DateTime.UtcNow;
                SelectedStatus = "Open";
            }

            await ExecutePopulateControlListAsync();

            if (Activity != null)
            {
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

            await PopulateListsAsync();
        });
    }

    /// <summary>
    /// Gets the activity control identifier.
    /// </summary>
    /// <returns></returns>
    public async Task ExecutePopulateControlListAsync()
    {
        var controlTypes = await lookupService.GetControlTypeLookups();
        ActivityControlType = controlTypes.Where(c => c.Value == "Activity").FirstOrDefault()?.LookupId ?? 0;
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

    // <summary>
    /// Validates the punch item for save.
    /// </summary>
    /// <param name="punchItemToValidate">The punch item to validate.</param>
    /// <returns></returns>
    private async Task<string> ValidatePunchItemForSave(PunchItem punchItemToValidate)
    {
        var responseMessage = "";

        await Task.Run(() =>
        {
            if (punchItemToValidate.Description == null)
            {
                responseMessage = responseMessage + Environment.NewLine + "You must enter a description";
            }
            else
            {
                if (punchItemToValidate.Description.Length == 0)
                {
                    responseMessage = responseMessage + Environment.NewLine + "You must enter a description";
                }
            }

            if (!punchItemToValidate.PUCId.HasValue)
            {
                responseMessage = responseMessage + Environment.NewLine + "You must select a commissioning system";
            }

            if (!punchItemToValidate.PUId.HasValue)
            {
                responseMessage = responseMessage + Environment.NewLine + "You must select a unit";
            }

            if (punchItemToValidate.TagId == null)
            {
                responseMessage = responseMessage + Environment.NewLine + "You must select a component tag";
            }

            if (punchItemToValidate.ComponentType == null)
            {
                responseMessage = responseMessage + Environment.NewLine + "You must select a component type";
            }

            if (punchItemToValidate.Status == 0)
            {
                responseMessage = responseMessage + Environment.NewLine + "You must select a Status";
            }

            if (punchItemToValidate.Priority == null)
            {
                responseMessage = responseMessage + Environment.NewLine + "You must select a Priority";
            }

            if (punchItemToValidate.Priority.Value == 0)
            {
                responseMessage = responseMessage + Environment.NewLine + "You must select a Priority";
            }

            if (punchItemToValidate.DisciplineId == null || punchItemToValidate.DisciplineId == 0)
            {
                responseMessage = responseMessage + Environment.NewLine + "You must select a Discipline";
            }
        });

        return responseMessage;

    }

    #endregion

    #region [ Commands ]

    [RelayCommand]
    public async Task PopulateProjects()
    {
        Projects.Clear();

        availableProjects = await lookupService.GetProjectListAsync();

#if DEBUG
        var list = availableProjects.ToList();

        // Add new item
        list.Add(new Project
        {
            ProjectId = 2,
            Name = "Test Project",
            Description = "Test Project Added for Testiing in Dev Env",
            Enabled = true
        });

        // Assign back if needed
        availableProjects = list;
#endif

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
                Punch.ProjectId = availableProjects.FirstOrDefault()?.ProjectId ?? 0;
            }

            SelectedProject = availableProjects?
                    .FirstOrDefault(p => p.ProjectId == Punch.ProjectId)?
                    .Name ??
                    string.Empty;

            // if we have a valid project Id then we need to populate the priority list
            if (Punch.ProjectId != 0)
            {
                await PopulatePriorityListCommand.ExecuteAsync(Punch.ProjectId);
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

    [RelayCommand]
    private async Task ExecutePunchImage()
    {
        var param = new Dictionary<string, object>
            {
                {NavigationParamConstant.FileType, FileType.Punch},
                { NavigationParamConstant.Punch, Punch }
            };

        await NavigationService.NavigateToPage<FileListPage>(parameters: param);
    }

    [RelayCommand]
    private async Task FetchUnits()
    {

        if (Punch != null && Punch.ProjectId != 0)
        {
            availableUnits = await lookupService
                .GetUnitListAsync();

            if (availableUnits != null && availableUnits.Count() > 0)
            {
                var availableUnitNames = availableUnits
                    .Where(p => p.ProjectId == Punch.ProjectId)
                    .OrderBy(p => p.Name)
                    .Select(p => p.Name);

                Units = new ObservableCollection<string>(availableUnitNames!);

                var unit = availableUnits.FirstOrDefault(p => p.PUId == Punch.PUId);
                SelectedUnit = unit?.Name;
            }
        }
    }

    [RelayCommand]
    private async Task FetchCommSystems()
    {
        if (Punch != null && Punch.PUId != 0)
        {
            availableCommSystems = await lookupService
                .GetCommSystemListAsync();

            var PUId = (availableUnits != null && !string.IsNullOrEmpty(SelectedUnit))
               ? availableUnits.FirstOrDefault(p => p.Name == SelectedUnit && p.ProjectId == Punch.ProjectId)?.PUId : 0;


            var availableCommSystemNames = availableCommSystems
                .Where(p => p.ProjectId == Punch.ProjectId && p.PUId == PUId)
                .OrderBy(p => p.Name)
                .Select(p => p.Name);

            CommSystems = new ObservableCollection<string>(availableCommSystemNames);


            var commSystem = availableCommSystems.FirstOrDefault(p => p.PUCId == Punch.PUCId);

            SelectedCommSystem = commSystem != null ? commSystem.Name : string.Empty;
        }
    }

    [RelayCommand]
    private async Task FetchComponentTypes()
    {
        availableComponents = await lookupService
                            .GetComponentListAsync();

        availableCommSystems = await lookupService.GetCommSystemListAsync();


        var PUId = (availableUnits != null && !string.IsNullOrEmpty(SelectedUnit))
                         ? availableUnits.FirstOrDefault(p => p.Name == SelectedUnit && p.ProjectId == Punch.ProjectId)?.PUId : 0;

        var PUCId = (availableCommSystems != null && !string.IsNullOrEmpty(SelectedCommSystem)) ? availableCommSystems
            .FirstOrDefault(p => p.Name == SelectedCommSystem && p.PUId == PUId)?.PUCId : 0;

        var availableComponentTypeNames = availableComponents
                .Where(p => p.ProjectId == Punch.ProjectId && p.PUCId == PUCId)
                .OrderBy(p => p.Name)
                .Select(p => p.Name)
                .Distinct();

        ComponentTypes = new ObservableCollection<string>(availableComponentTypeNames);


        SelectedComponentType = Punch.ComponentType;
    }

    [RelayCommand]
    private async Task FetchComponentTags()
    {
        availableComponents = await lookupService
                            .GetComponentListAsync();


        var PUCId = (availableCommSystems != null && !string.IsNullOrEmpty(SelectedCommSystem)) ? availableCommSystems
          .FirstOrDefault(p => p.Name == SelectedCommSystem)?.PUCId : 0;

        var availableComponentTagIds = availableComponents
            .Where(p => p.ProjectId == Punch.ProjectId && p.Name == Punch.ComponentType)
            .Where(p => p.PUCId == PUCId)
               .OrderBy(p => p.TagId)
            .Select(p => p.TagId)
            .Distinct();

        ComponentTags = new ObservableCollection<string>(availableComponentTagIds);

        SelectedComponentTag = Punch.TagId;
    }

    [RelayCommand]
    private async Task FetchActivities()
    {
        availableActivities = await lookupService.GetActivtyListAsync();

        if (availableActivities.Count() > 0)
        {
            try
            {
                var tagId = SelectedComponentTag;

                var availableActivityNames = availableActivities
                        .Where(a => a.ProjectId == Punch.ProjectId && a.TagId == tagId)
                        .OrderBy(a => a.Name)
                        .Select(a => a.Name);

                ActivitiesName = new ObservableCollection<string>(availableActivityNames);


                var PCAId = (availableCommSystems != null && !string.IsNullOrEmpty(SelectedComponentTag)) ? availableActivities
                .FirstOrDefault(p => p.TagId == SelectedComponentTag)?.PCAId : 0;

                if (CreatedFromActivityItem)
                {
                    if (Activity != null)
                    {
                        SelectedActivity = Activity.Name;

                        // set the lead discipline from the activity
                        var disciplineItems = await disciplineService.GetDisciplineListAsync();

                        // if the activity has a lead discipline use it to set the punch discipline
                        if (Activity.LeadDisciplineId != null)
                        {
                            var selected = disciplineItems.FirstOrDefault(p => p.DisciplineId == Activity.LeadDisciplineId);

                            if (selected != null)
                            {
                                SelectedDiscipline = selected.Name;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }

    [RelayCommand]
    private async Task SavePunch()
    {
        // Populate values on punch item
        if (newPunch)
        {
            Punch.PUId = (availableUnits != null && !string.IsNullOrEmpty(SelectedUnit))
               ? availableUnits.FirstOrDefault(p => p.Name == SelectedUnit && p.ProjectId == Punch.ProjectId)?.PUId : 0;

            Punch.PUCId = (availableCommSystems != null && !string.IsNullOrEmpty(SelectedCommSystem)) ? availableCommSystems
           .FirstOrDefault(p => p.Name == SelectedCommSystem)?.PUCId : 0;

            Punch.PCCId = (availableComponents != null && !string.IsNullOrEmpty(SelectedComponentTag)) ? availableComponents
                        .FirstOrDefault(p => p.TagId == SelectedComponentTag && p.PUCId == Punch.PUCId && p.Name == SelectedComponentType)?.PCCId : 0;


            Punch.TagId = SelectedComponentTag;

            if (!String.IsNullOrEmpty(SelectedActivity))
            {
                Punch.PCAId = (availableActivities != null && !string.IsNullOrEmpty(SelectedActivity)) ? availableActivities
                .FirstOrDefault(p => p.Name == SelectedActivity && p.PCCId == Punch.PCCId)?.PCAId : 0;
            }


        }

        var validationMessage = await ValidatePunchItemForSave(Punch);

        if (validationMessage.Length == 0)
        {
            await punchService.SavePunchItem(Punch);

            //Reload the punch data
            await punchService.FetchPunchListAsync();

            Punch = punchService
                .Punches
                .FirstOrDefault(p => p.Id == Punch.Id);

            await DialogService.ShowAlertDialog("Suceess!!", UserInterface.ActivityPage_Saved, Enums.AlertType.Success);
        }
        else
        {
            await DialogService.ShowAlertDialog("Error!!", validationMessage, Enums.AlertType.Error);
        }
    }

    [RelayCommand]
    private async Task CancelPunch()
    {
        await OnBackPressed();
    }

    #endregion

    #region [ Override Methods ]

    public override void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        base.ApplyQueryAttributes(query);
        if (query.ContainsKey(NavigationParamConstant.Punch))
        {
            IsNewPunch = false;
            Punch = (PunchItem)query[NavigationParamConstant.Punch];
        }
        else
        {
            IsNewPunch = true;
            query.TryGetValue(NavigationParamConstant.Activity, out object? arg);
            Activity = arg != null ? (Activity)arg : null;

        }
        InitializeData();
    }


    #endregion

}
