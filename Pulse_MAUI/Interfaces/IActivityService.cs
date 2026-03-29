using Pulse_MAUI.Helpers;
using Pulse_MAUI.Models;

namespace Pulse_MAUI.Interfaces
{
    public interface IActivityService
    {
        ObservableRangeCollection<Activity>? Activities { get; }

        Task FetchActivityListAsync();
        Task<ObservableRangeCollection<Activity>> FetchFilteredActivitiesList();
        Task<IEnumerable<ActivityTask>> FetchActivityTasksAsync(Activity activity);
        Task<bool> ActivityNeedsSave(Activity activity);
        Task<bool> ActivityTaskNeedsSave(ActivityTask activityTask);
        Task SaveActivity(Activity activityToSave);
        Task SaveActivityTasks(IEnumerable<ActivityTask> activityTasksToSave);
        Task<int> GetExistingActivityTaskStatusId(string activityId);
    }
}