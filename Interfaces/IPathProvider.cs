namespace Pulse_MAUI.Interfaces
{
    public interface IPathProvider
    {
        string GetDownloadPath();
        Task<string> GetFileFromUrl(string url);
    }
}
