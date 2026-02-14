
namespace Pulse_MAUI.Interfaces
{
    public interface IConnectivityService
    {
        bool IsConnected { get;}
        Task CheckConnected();
    }
}