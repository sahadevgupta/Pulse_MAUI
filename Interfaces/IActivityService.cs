using Pulse_MAUI.Helpers;
using Pulse_MAUI.Models;

namespace Pulse_MAUI.Interfaces
{
    public interface IActivityService
    {
        Task FetchActivityListAsync();
        Task<ObservableRangeCollection<Activity>> FetchFilteredActivitiesList();
    }
}