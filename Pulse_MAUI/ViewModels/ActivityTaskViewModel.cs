using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Pulse_MAUI.Helpers;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models;
using Pulse_MAUI.Resources.Languages;

namespace Pulse_MAUI.ViewModels;

public partial class ActivityTaskViewModel(IViewModelParameters viewModelParameters,
IEquipmentService equipmentService,
    ILookupService lookupService) : BaseViewModel(viewModelParameters)
{


    [ObservableProperty]
    ActivityTask? _activityTask;

    [ObservableProperty]
    ObservableCollection<string> _statusList = new();

    [ObservableProperty]
    ObservableCollection<Equipment> _equipmentList = new();

    [ObservableProperty]
    string? _selectedStatus;

    [ObservableProperty]
    Equipment? _selectedEquipment;

    /// <summary>
    /// Gets the task title.
    /// </summary>
    /// <value>The task title.</value>
    public string TaskTitle
    {
        get
        {
            return string.Format(UserInterface.ActivityPage_StepHeader,
                                 ActivityTask.Step,
                                 ActivityTask.Status != null ?
                                 AsyncHelpers.RunSync(async () => await lookupService.GetLookupValue(ActivityTask.Status.Value)) :
                                     string.Empty
                                 );
        }
    }

    partial void OnSelectedStatusChanged(string? oldValue, string? newValue)
    {
        if (ActivityTask != null)
        {
            ActivityTask.Status = AsyncHelpers.RunSync(async () => await lookupService.GetActivityTaskStatusLookupId(SelectedStatus));
        }
    }

    partial void OnSelectedEquipmentChanged(Equipment? oldValue, Equipment? newValue)
    {
        if (ActivityTask != null)
        {
            ActivityTask.Equipment = newValue.EquipmentId;
            OnPropertyChanged(nameof(TaskTitle));
        }
    }

    /// <summary>
    /// Fetches the list of status available for the picker.
    /// </summary>
    /// <returns>The status list.</returns>
    public async Task FetchStatusList()
    {
        var availableStatusList = await lookupService.GetActivityTaskStatusLookups();

        foreach (var availableStatus in availableStatusList)
        {
            StatusList.Add(availableStatus.Value);
        }

        var status = availableStatusList
            .FirstOrDefault(p => p.LookupId == ActivityTask.Status);

        SelectedStatus = status != null ? status.Value : string.Empty;
    }

    /// <summary>
    /// Fetches the equipment list.
    /// </summary>
    /// <returns></returns>
    public async Task FetchEquipmentList()
    {
        // Get the equipment list and assign it
        EquipmentList = await equipmentService.FetchEquipmentTasksAsync(ActivityTask.ProjectId);
        SelectedEquipment = EquipmentList.FirstOrDefault(E => E.EquipmentId == ActivityTask.Equipment);
    }


}
