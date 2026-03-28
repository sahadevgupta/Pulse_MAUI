using Pulse_MAUI.Models;

namespace Pulse_MAUI.Interfaces
{
    public interface IEngineerService
    {
        Engineer CurrentEngineer { get; set; }

        Task FetchCurrentEngineer();
    }
}