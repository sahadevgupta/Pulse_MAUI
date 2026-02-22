using Pulse_MAUI.Helpers;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models;

namespace Pulse_MAUI.Services
{
    public class ActivityService(IDataManager dataManager, IActivitySearchService activitySearchService) : IActivityService
    {
        /// <summary>
        /// The activities.
        /// </summary>
        public ObservableRangeCollection<Activity>? Activities { get; set; }

        /// <summary>
        /// Fetchs the assigned activities and populates the Activity collection.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task FetchActivityListAsync()
        {
            //Activities.Clear();
            Activities = new ObservableRangeCollection<Activity>();

            IEnumerable<Activity> availableActivities = await dataManager.GetAllActivitiesAsync();
            Activities.AddRange(availableActivities, System.Collections.Specialized.NotifyCollectionChangedAction.Add);
        }




        /// <summary>
        /// Filters the assigned activity list depending on search options selected.
        /// </summary>
        public async Task<ObservableRangeCollection<Activity>> FetchFilteredActivitiesList()
        {

            var AFiltered = new ObservableRangeCollection<Activity>();

            await FetchActivityListAsync();
            // get the search items
            bool result = await activitySearchService.FetchSearchItems(Activities);

            try
            {

                IEnumerable<Activity> availableActivitiesMain = await dataManager.GetAllActivitiesAsync();
                var availableActivities = availableActivitiesMain.OrderByDescending(o => o.status).ToList();

                //TODO: Need to check this logic

                //if (ActivitySearchService.HasOptionSelected(ActivitySearchService.Instance.SelectedUnit))
                //{
                //    availableActivities = availableActivities
                //        .Where(p => p.Unit == ActivitySearchService.Instance.SelectedUnit)
                //        .ToList();
                //}


                //if (ActivitySearchService.HasOptionSelected(ActivitySearchService.Instance.SelectedCommSystem))
                //{
                //    availableActivities = availableActivities
                //        .Where(p => p.CommissioningSystem == ActivitySearchService.Instance.SelectedCommSystem)
                //        .ToList();
                //}


                //if (ActivitySearchService.HasOptionSelected(ActivitySearchService.Instance.SelectedComponentType))
                //{
                //    availableActivities = availableActivities
                //        .Where(p => p.ComponentType == ActivitySearchService.Instance.SelectedComponentType)
                //        .ToList();
                //}

                //if (ActivitySearchService.HasOptionSelected(ActivitySearchService.Instance.SelectedComponentTag))
                //{
                //    availableActivities = availableActivities
                //        .Where(p => p.TagId == ActivitySearchService.Instance.SelectedComponentTag)
                //        .ToList();
                //}

                //if (ActivitySearchService.HasOptionSelected(ActivitySearchService.Instance.SelectedActivity))
                //{
                //    availableActivities = availableActivities
                //        .Where(p => p.Name == ActivitySearchService.Instance.SelectedActivity)
                //        .ToList();
                //}

                AFiltered.AddRange(availableActivities, System.Collections.Specialized.NotifyCollectionChangedAction.Add);
                return AFiltered;
            }
            catch
            {
                return AFiltered;
            }

            //private static ActivityService? _instance;

            ///// <summary>
            ///// Gets the instance.
            ///// </summary>
            ///// <value>The instance.</value>
            //public static ActivityService Instance
            //{
            //    get
            //    {
            //        if (_instance == null)
            //        {
            //            _instance = new ActivityService();
            //        }

            //        return _instance;
            //    }
            //}



            ///// <summary>
            ///// The activities filtered.
            ///// </summary>
            //public ObservableRangeCollection<Activity>? ActivitiesFiltered = new ObservableRangeCollection<Activity>();



            ///// <summary>
            ///// Determines whether [is location available].
            ///// </summary>
            ///// <returns>
            /////   <c>true</c> if [is location available]; otherwise, <c>false</c>.
            ///// </returns>
            //public bool IsLocationAvailable()
            //{

            //    if (Keys.UseLocationServices)
            //    {
            //        return Geolocation.IsEnabled;
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}

            ///// <summary>
            ///// Gets the activty list asynchronous.
            ///// </summary>
            ///// <returns></returns>
            //public async Task<IEnumerable<Activity>> GetActivtyListAsync()
            //{
            //    return await dataManager
            //        .GetAllActivitiesAsync();
            //}


            ///// <summary>
            ///// Gets the activity.
            ///// </summary>
            ///// <param name="activityName">Name of the activity.</param>
            ///// <returns></returns>
            //public async Task<Activity> GetActivity(string activityName)
            //{

            //    if (Activities.Count == 0)
            //    {
            //        await FetchActivityListAsync();
            //    }

            //    return Activities.Where(a => a.Name == activityName).FirstOrDefault();

            //}

            ///// <summary>
            ///// Gets the activity.
            ///// </summary>
            ///// <param name="PCAId">The pca identifier.</param>
            ///// <returns></returns>
            //public async Task<Activity> GetActivity(int? PCAId)
            //{
            //    if (Activities.Count == 0)
            //    {
            //        await FetchActivityListAsync();
            //    }

            //    return Activities.Where(a => a.PCAId == PCAId).FirstOrDefault();
            //}




            //}

            ///// <summary>
            ///// Returns the activity task for an activity.
            ///// </summary>
            ///// <returns>The activity tasks for a activity async.</returns>
            ///// <param name="activity">Activity.</param>
            //public async Task<IEnumerable<ActivityTask>> FetchActivityTasksAsync(Activity activity)
            //{
            //    var availableActivityTasks = await dataManager.GetAllActivityTasksForActivityAsync(activity);

            //    var orderedActivityTasks = availableActivityTasks
            //        .OrderBy(p => p.Step)
            //        .ToList();

            //    return orderedActivityTasks;
            //}

            ///// <summary>
            ///// Saves a list of activity tasks.
            ///// </summary>
            ///// <returns>Task.</returns>
            ///// <param name="activityTasksToSave">Activity Tasks to save.</param>
            //public async Task SaveActivityTasks(IEnumerable<ActivityTask> activityTasksToSave)
            //{

            //    //float? LastLatitude;
            //    //float? LastLongitude;

            //    //// just get the location once for the tasks
            //    //if (IsLocationAvailable())
            //    //{
            //    //    var locator = CrossGeolocator.Current;
            //    //    var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));

            //    //    LastLatitude = (float)position.Latitude;
            //    //    LastLongitude = (float)position.Longitude;
            //    //}
            //    //else
            //    //{
            //    //    LastLatitude = null;
            //    //    LastLongitude = null;
            //    //}

            //    foreach (var activityTaskToSave in activityTasksToSave)
            //    {


            //        if (await ActivityTaskNeedsSave(activityTaskToSave))
            //        {
            //            activityTaskToSave.Engineer = EngineerService.Instance.CurrentEngineer.EngineerId;
            //            activityTaskToSave.DateRecorded = DateTime.UtcNow;
            //            activityTaskToSave.UserName = UserService.Instance.CurrentUser.ApexId;


            //            //if (LastLatitude != null)
            //            //{
            //            //    activityTaskToSave.LastLatitude = (float)LastLatitude;
            //            //}

            //            //if (LastLongitude != null)
            //            //{
            //            //    activityTaskToSave.LastLongitude = (float)LastLongitude;
            //            //}


            //            await dataManager.SaveActivityTaskAsync(activityTaskToSave);
            //        }
            //    }
            //}

            ///// <summary>
            ///// Saves the activity.
            ///// </summary>
            ///// <param name="activityToSave">The activity to save.</param>
            ///// <returns></returns>
            //public async Task SaveActivity(Activity activityToSave)
            //{

            //    //float? LastLatitude;
            //    //float? LastLongitude;

            //    // TODO: Need to check why location is needed.
            //    // just get the location once for the tasks

            //    //if (IsLocationAvailable())
            //    //{
            //    //    var locator = CrossGeolocator.Current;
            //    //    var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));

            //    //    LastLatitude = (float)position.Latitude;
            //    //    LastLongitude = (float)position.Longitude;
            //    //}
            //    //else
            //    //{
            //    //    LastLatitude = null;
            //    //    LastLongitude = null;
            //    //}

            //    if (await ActivityNeedsSave(activityToSave))
            //    {
            //        activityToSave.DateRecorded = DateTime.UtcNow;
            //        activityToSave.ModifiedBy = UserService.Instance.CurrentUser.ApexId;
            //        //activityToSave.LastLatitude = LastLatitude;
            //        //activityToSave.LastLongitude = LastLongitude;


            //        await dataManager.SaveActivityAsync(activityToSave);
            //    }
            //}




            ///// <summary>
            ///// Check if Activity Requires a Save
            ///// </summary>
            ///// <param name="activity">The activity.</param>
            ///// <returns></returns>
            //public async Task<bool> ActivityNeedsSave(Activity activity)
            //{
            //    bool needsSaving = false;

            //    if (!string.IsNullOrEmpty(activity.Id))
            //    {
            //        var savedActivity = await dataManager.GetActivityById(activity.Id);

            //        if (activity.StatusId != savedActivity.StatusId)
            //        {
            //            needsSaving = true;
            //        }
            //    }
            //    else
            //    {
            //        needsSaving = true;
            //    }

            //    return needsSaving;

            //}




            ///// <summary>
            ///// Checks whether the activity task has been updated.
            ///// </summary>
            ///// <returns>The task needs save.</returns>
            ///// <param name="activityTask">Activity task.</param>
            //public async Task<bool> ActivityTaskNeedsSave(ActivityTask activityTask)
            //{
            //    bool needsSaving = false;

            //    if (!string.IsNullOrEmpty(activityTask.Id))
            //    {
            //        var savedActivityTask = await dataManager.GetActivityTaskById(activityTask.Id);


            //        if (activityTask.ActualResult != savedActivityTask.ActualResult ||
            //            activityTask.Comments != savedActivityTask.Comments ||
            //            activityTask.Status != Helpers.General.NullableIntegerToInt(savedActivityTask.Status, 0) || activityTask.Equipment != savedActivityTask.Equipment)
            //        {
            //            needsSaving = true;
            //        }
            //    }
            //    else
            //    {
            //        needsSaving = true;
            //    }

            //    return needsSaving;
            //}

            ///// <summary>
            ///// Gets the existing activity task status identifier.
            ///// </summary>
            ///// <param name="activityId">The activity identifier.</param>
            ///// <returns></returns>
            //public async Task<int> GetExistingActivityTaskStatusId(string activityId)
            //{
            //    var savedActivity = await dataManager.GetActivityById(activityId);

            //    return savedActivity.StatusId;
            //}


        }
    }
}
