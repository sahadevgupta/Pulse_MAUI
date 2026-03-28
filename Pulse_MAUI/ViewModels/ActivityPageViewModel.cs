using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pulse_MAUI.Constants;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models;

namespace Pulse_MAUI.ViewModels;

[QueryProperty(nameof(Activity), NavigationParamConstant.Activity)]
public partial class ActivityPageViewModel(IViewModelParameters viewModelParameters,
    ILookupService lookupService,
    IActivityService activityService) : BaseViewModel(viewModelParameters)
{
    #region [ Properties ]

    [ObservableProperty]
    private Activity? _activity;

    [ObservableProperty]
    private ObservableCollection<ActivityTask>? _activityTasks;

    [ObservableProperty]
    private ObservableCollection<string>? _statusList;

    [ObservableProperty]
    private string? _selectedStatus;

    [ObservableProperty]
    private int _taskCount;


    #endregion

    #region [ Methods & Service Calls ]

    partial void OnSelectedStatusChanged(string? oldValue, string? newValue)
    {
        if (Activity != null)
        {
            //Activity.StatusId = AsyncHelpers.RunSync(async () => await LookupService.Instance.GetActivityStatusLookupId(selectedStatus));

            Activity.Status = newValue;


        }

    }

    public async Task InitializeDataAsync()
    {

        DialogService.ShowLoading("Loading Tasks");

        // if (ViewModel.ActivityTasks.Count == 0)
        // {
        //     //ViewModel.FetchDataCommand.Execute(null);
        //     await ViewModel.ExecuteFetchDataCommandAsync();
        // }
        // else
        // {
        //     await ViewModel.UpdateImageCount();
        // }

        DialogService.HideLoading();
    }

    /// <summary>
    /// Fetches the list of status available for the picker.
    /// </summary>
    /// <returns>The status list.</returns>
    private async Task FetchActivityStatusListAsync()
    {
        //StatusList.Clear();

        var availableStatusList = await lookupService.GetActivityStatusLookups();

        StatusList = new ObservableCollection<string>(availableStatusList.Select(s => s.Value)!);


        var status = availableStatusList.FirstOrDefault(p => p.LookupId == Activity?.StatusId);

        SelectedStatus = status != null ? status.Value : string.Empty;
    }

    /// <summary>
    /// Fetches the activity tasks from the service and populates the collection.
    /// </summary>
    /// <returns>async task.</returns>
    private async Task<IEnumerable<ActivityTask>> FetchActivityTasksAsync()
    {

        var availableActivityTasks = await activityService.FetchActivityTasksAsync(Activity!);
        ActivityTasks = new ObservableCollection<ActivityTask>(availableActivityTasks);

        // set the task count value;
        Activity?.TaskCount = availableActivityTasks.Count();
        TaskCount = availableActivityTasks.Count();

        return availableActivityTasks;

    }
    /// <summary>
    /// Fetch a count of the associated blobs
    /// </summary>
    /// <returns>async task.</returns>
    private async Task FetchImageCountAsync()
    {
        // await FileService.Instance.FetchItemFiles(activity);
        // if (FileService.Instance.ActivityFiles != null)
        // {
        //     await Task.Run(() =>
        //     {
        //         ImagesCount = FileService.Instance.ActivityFiles.Count();
        //     });
        // }

    }

    /// <summary>
    /// Fetches all status lists
    /// </summary>
    /// <returns>async task.</returns>
    private async Task FetchStatusListsAsync(IEnumerable<ActivityTask> activityTasks)
    {

        foreach (var activityTask in activityTasks)
        {
            //await activityTask.FetchStatusList();
        }
    }

    /// <summary>
    /// Fetches the equipment list asynchronous.
    /// </summary>
    /// <returns></returns>
    private async Task FetchEquipmentListAsync(IEnumerable<ActivityTask> activityTasks)
    {
        foreach (var activityTask in activityTasks)
        {
            //await activityTask.FetchEquipmentList();
        }
    }

    /// <summary>
    /// Fetches the list of status available for the picker.
    /// </summary>
    /// <returns>The status list.</returns>
    public async Task FetchStatusList()
    {
        //StatusList.Clear();
        var availableStatusList = await lookupService.GetActivityTaskStatusLookups();

        // foreach (var availableStatus in availableStatusList)
        // {
        //     StatusList.Add(availableStatus.Value);
        // }

        // var status = availableStatusList
        //     .FirstOrDefault(p => p.LookupId == ActivityTask.Status);

        // SelectedStatus = status != null ? status.Value : string.Empty;
    }

    #endregion

    #region [ Commands ]

    [RelayCommand]
    private async Task FetchData()
    {
        try
        {
            DialogService.ShowLoading();

            // 1. Run these three in parallel: status list, tasks, images
            var statusTask = FetchActivityStatusListAsync();
            var activityTasksTask = FetchActivityTasksAsync();
            var imageCountTask = FetchImageCountAsync();

            await Task.WhenAll(statusTask, activityTasksTask, imageCountTask);

            // 2. Now we have the tasks list from step 1
            var tasks = await activityTasksTask;

            // 3. Fetch per-task lists in parallel
            var fetchStatusListsTask = FetchStatusListsAsync(tasks);
            var fetchEquipmentListsTask = FetchEquipmentListAsync(tasks);

            await Task.WhenAll(fetchStatusListsTask, fetchEquipmentListsTask);
        }
        catch
        {

        }
        finally
        {
            DialogService.HideLoading();
        }
    }
    #endregion

}
