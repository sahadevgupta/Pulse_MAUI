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
using Pulse_MAUI.Views;

namespace Pulse_MAUI.ViewModels;

public partial class ActivityPageViewModel(IViewModelParameters viewModelParameters,
    IActivityService activityService,
    ILookupService lookupService,
    IFileService fileService) : BaseViewModel(viewModelParameters)
{
    #region [ Properties ]

    [ObservableProperty]
    private Activity? _activity;

    [ObservableProperty]
    private ObservableCollection<ActivityTaskViewModel> _activityTasks = new();

    [ObservableProperty]
    private ObservableCollection<string> _statusList = new();

    [ObservableProperty]
    private string? _selectedStatus;

    [ObservableProperty]
    private int _taskCount;

    [ObservableProperty]
    private int _imagesCount;


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

    public void InitializeData()
    {
        DialogService.ShowLoading();
        Task.Run(async () =>
        {

            try
            {
                if (ActivityTasks.Count == 0)
                {
                    //ViewModel.FetchDataCommand.Execute(null);
                    await FetchDataCommand.ExecuteAsync(null);
                }
                else
                {
                    await FetchImageCountAsync();
                }
            }
            catch
            {

            }
            finally
            {
                DialogService.HideLoading();
            }

        });
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
    private async Task<IEnumerable<ActivityTaskViewModel>> FetchActivityTasksAsync()
    {
        List<ActivityTaskViewModel> tasks = new List<ActivityTaskViewModel>();
        var availableActivityTasks = await activityService.FetchActivityTasksAsync(Activity!);
        foreach (var availableActivityTask in availableActivityTasks)
        {
            tasks.Add(ConvertToViewModel(availableActivityTask));
        }
        ActivityTasks = new ObservableCollection<ActivityTaskViewModel>(tasks);

        // set the task count value;
        Activity?.TaskCount = availableActivityTasks.Count();
        TaskCount = availableActivityTasks.Count();

        return tasks;

    }
    /// <summary>
    /// Fetch a count of the associated blobs
    /// </summary>
    /// <returns>async task.</returns>
    private async Task FetchImageCountAsync()
    {
        var activityFiles = await fileService.FetchItemFiles(Activity!);
        if (activityFiles.Any())
            ImagesCount = activityFiles.Count();
    }

    /// <summary>
    /// Fetches all status lists
    /// </summary>
    /// <returns>async task.</returns>
    private async Task FetchStatusListsAsync(IEnumerable<ActivityTaskViewModel> activityTasks)
    {

        foreach (var activityTask in activityTasks)
        {
            await activityTask.FetchStatusList();
        }
    }

    /// <summary>
    /// Fetches the equipment list asynchronous.
    /// </summary>
    /// <returns></returns>
    private async Task FetchEquipmentListAsync(IEnumerable<ActivityTaskViewModel> activityTasks)
    {
        foreach (var activityTask in activityTasks)
        {
            await activityTask.FetchEquipmentList();
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

    /// <summary>
    /// Converts an activity task to a activitytaskviewmodel.
    /// </summary>
    /// <returns>Activity task view model.</returns>
    /// <param name="activityTask">Activity task.</param>
    private ActivityTaskViewModel ConvertToViewModel(ActivityTask activityTask)
    {
        var activityViewModel = ServiceHelper.GetService<ActivityTaskViewModel>();
        activityViewModel.ActivityTask = activityTask;

        return activityViewModel;
    }

    #endregion

    #region [ Commands ]

    [RelayCommand]
    private async Task FetchData()
    {
        try
        {
            // 1. Run these three in parallel: status list, tasks, images
            await FetchActivityStatusListAsync();
            var tasks = await FetchActivityTasksAsync();
            await FetchImageCountAsync();


            // 3. Fetch per-task lists in parallel
            await FetchStatusListsAsync(tasks);
            await FetchEquipmentListAsync(tasks);

        }
        catch
        {

        }
        finally
        {
            //DialogService.HideLoading();
        }
    }

    [RelayCommand]
    private async Task CloseActivity()
    {
        // Close the modal and return to ActivityListPage
        if (Shell.Current?.CurrentPage?.Navigation != null)
        {
            await Shell.Current.CurrentPage.Navigation.PopModalAsync();
        }
    }

    [RelayCommand]
    private async Task ExecuteActivityImage()
    {
        if (IsBusy)
            return;

        IsBusy = true;

        DialogService.ShowLoading();

        var activityTasksToSave = ActivityTasks
                    .ToList()
                    .Select(p => p.ActivityTask);


        var needsSave = false;
        foreach (ActivityTask activityTask in activityTasksToSave)
        {
            if (await activityService.ActivityTaskNeedsSave(activityTask))
            {
                needsSave = true;
            }

            if (await activityService.ActivityNeedsSave(Activity))
            {
                needsSave = true;
            }

        }

        if (needsSave)
        {

            // display a notification
            await DialogService.ShowAlertDialog(UserInterface.ActivityPage_PromptForSaveTitle, UserInterface.SaveWarningMessage);

            DialogService.HideLoading();
        }
        else
        {
            var param = new Dictionary<string, object>
            {
                {NavigationParamConstant.FileType, FileType.Activity},
                { NavigationParamConstant.Activity, Activity }
            };

            await NavigationService.NavigateToPage<FileListPage>(parameters: param);

            DialogService.HideLoading();

        }

        IsBusy = false;

    }

    [RelayCommand]
    private async Task SaveActivity()
    {
        bool _success = true;

        var activityTasksToSave = ActivityTasks
            .ToList()
            .Select(p => p.ActivityTask);


        // check if we are requesting close complete
        if (Activity.Status.Contains("closed", StringComparison.OrdinalIgnoreCase))
        {
            // check all the steps are passed (greater than 0)
            var totalTasks = activityTasksToSave.Count();
            var closedTasks = activityTasksToSave.Where(at => at.Status > 0).Count();

            if (totalTasks != closedTasks)
            {
                _success = false;
                await DialogService.ShowAlertDialog("Alert!!", "The status on all activity tasks must be set.", Enums.AlertType.Error);
            }


        }

        if (_success)
        {
            Activity.StatusId = AsyncHelpers.RunSync(async () => await lookupService.GetActivityStatusLookupId(SelectedStatus));
            Activity.Status = SelectedStatus;

            await activityService.SaveActivityTasks(activityTasksToSave);


            // Compare the old status ID with the new to see if any changes have been made.
            int oldStatusId = await activityService.GetExistingActivityTaskStatusId(Activity.Id);

            if (Activity.StatusId != oldStatusId)
            {
                // if so, check if we need to change the date completed value.
                if (Activity.Status.ToLower().Contains("closed"))
                {
                    Activity.DateCompleted = DateTime.UtcNow;
                }
                else
                {
                    Activity.DateCompleted = null;
                }
            }

            await activityService.SaveActivity(Activity);


            await DialogService.ShowAlertDialog("Suceess!!", UserInterface.ActivityPage_Saved, Enums.AlertType.Success);
        }
    }

    [RelayCommand]
    private async Task NavigateToPunch()
    {
        await NavigationService.NavigateToPage<PunchListPage>();
    }

    [RelayCommand]
    private async Task CancelActivity()
    {
        if (IsBusy)
            return;

        IsBusy = true;

        var needsSave = false;

        var activityTasksToSave = ActivityTasks
         .ToList()
         .Select(p => p.ActivityTask);

        if (await activityService.ActivityNeedsSave(Activity))
        {
            needsSave = true;
        }

        foreach (var activityTask in activityTasksToSave)
        {
            if (await activityService.ActivityTaskNeedsSave(activityTask))
            {
                needsSave = true;
            }

        }


        if (needsSave)
        {
            var confirm = await Shell.Current.DisplayAlertAsync(
                UserInterface.ActivityPage_PromptForSaveTitle,
                    UserInterface.ActivityPage_PromptForSaveTitle,
                    UserInterface.ConfirmYes,
                    UserInterface.ConfirmNo
                );


            if (confirm)
            {
                //Reload the punch data
                //ResetActivityItemsCommand.Execute(this);
                await NavigationService.NavigateBack();
            }
        }
        else
        {
            await NavigationService.NavigateBack();
        }

        IsBusy = false;
    }


    #endregion

    #region [ Override Methods ]

    public override void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey(NavigationParamConstant.Activity))
        {
            Activity = (Activity)query[NavigationParamConstant.Activity];
            InitializeData();
        }
    }


    #endregion
}
