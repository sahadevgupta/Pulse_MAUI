using Pulse_MAUI.Models;

namespace Pulse_MAUI.Interfaces
{
    public interface ILookupService
    {
        Task<IEnumerable<Lookup>> GetControlTypeLookups();
        Task<IEnumerable<Component>> GetComponentListAsync();
        Task<IEnumerable<CommissioningSystem>> GetCommSystemListAsync();
        Task<IEnumerable<Lookup>> GetLookupListAsync();
        Task<IEnumerable<Project>> GetProjectListAsync();
        Task<IEnumerable<Lookup>> GetStatusLookups();
        Task<IEnumerable<Unit>> GetUnitListAsync();
        Task<IEnumerable<Lookup>> GetActivityStatusLookups();
        Task<IEnumerable<Lookup>> GetActivityTaskStatusLookups();
        Task<IEnumerable<Activity>> GetActivtyListAsync();
        Task<int> GetStatusLookupId(string value);
        Task<int> GetActivityTaskStatusLookupId(string value);
        Task<int> GetActivityStatusLookupId(string value);
        Task<string?> GetLookupValue(int id);
    }
}