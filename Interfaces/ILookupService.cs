using Pulse_MAUI.Models;

namespace Pulse_MAUI.Interfaces
{
    public interface ILookupService
    {
        Task<IEnumerable<Lookup>> GetControlTypeLookups();
        Task<IEnumerable<CommissioningSystem>> GetCommSystemListAsync();
        Task<IEnumerable<Lookup>> GetLookupListAsync();
        Task<IEnumerable<Project>> GetProjectListAsync();
        Task<IEnumerable<Lookup>> GetStatusLookups();
        Task<IEnumerable<Unit>> GetUnitListAsync();
    }
}