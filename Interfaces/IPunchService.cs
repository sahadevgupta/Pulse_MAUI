using Pulse_MAUI.Helpers;
using Pulse_MAUI.Models;

namespace Pulse_MAUI.Interfaces
{
    public interface IPunchService
    {
        ObservableRangeCollection<PunchItem>? Punches { get;}

        Task FetchPunchListAsync();
        Task<IEnumerable<PunchItem>> GetPunchListAsync();
    }
}