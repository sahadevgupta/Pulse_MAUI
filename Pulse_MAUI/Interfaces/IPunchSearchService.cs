using Pulse_MAUI.Helpers;

namespace Pulse_MAUI.Interfaces
{
    public interface IPunchSearchService
    {
        ObservableRangeCollection<string> Activities { get; }
        ObservableRangeCollection<string> ComponentTags { get;}
        ObservableRangeCollection<string> ComponentTypes { get; }
        ObservableRangeCollection<string> CommSystem { get; }
        ObservableRangeCollection<string> Units { get; }

        ObservableRangeCollection<string> FetchActivities();
        ObservableRangeCollection<string> FetchActivitiesByComponentTag(string selectedComponentTag);
        ObservableRangeCollection<string> FetchCommSystem();
        ObservableRangeCollection<string> FetchCommSystemByUnit(string selectedUnit);
        ObservableRangeCollection<string> FetchComponentTags();
        ObservableRangeCollection<string> FetchComponentTagsByCompType(string selectedCompType);
        ObservableRangeCollection<string> FetchComponentTypes();
        ObservableRangeCollection<string> FetchComponentTypesByCommSystem(string selectedCommSystem);
        Task<bool> FetchSearchItems();
        ObservableRangeCollection<string> FetchUnits();
    }
}