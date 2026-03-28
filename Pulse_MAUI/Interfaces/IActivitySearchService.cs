using Pulse_MAUI.Helpers;
using Pulse_MAUI.Models;

namespace Pulse_MAUI.Interfaces
{
    public interface IActivitySearchService
    {
        ObservableRangeCollection<Activity>? Activities { get; }
        ObservableRangeCollection<string>? CommSystem { get; }
        ObservableRangeCollection<string>? Units { get; }
        ObservableRangeCollection<string>? ComponentTags { get; }
        ObservableRangeCollection<string>? ComponentTypes { get;}
        ObservableRangeCollection<string>? ActivitiesName { get;}
        Task<bool> FetchSearchItems(IEnumerable<Activity> activities);
        ObservableRangeCollection<string> FetchCommSystemByUnit(string selectedUnit);
        ObservableRangeCollection<string> FetchComponentTagsByCompType(string selectedCompType);
        ObservableRangeCollection<string> FetchComponentTypesByCommSystem(string selectedCommSystem);
        ObservableRangeCollection<string> FetchActivitiesByComponentTag(string selectedComponentTag);
        ObservableRangeCollection<string> FetchCommSystem();
        ObservableRangeCollection<string> FetchComponentTypes();
        ObservableRangeCollection<string> FetchComponentTags();
        ObservableRangeCollection<string> FetchActivities();
    }
}