namespace Pulse_MAUI.Interfaces
{
    public interface ISynchroniseService
    {
        Task DownloadBlobData(string ConnectionString);
        Task<List<string>> PushAndPullDataAsync(bool incremental, bool secondPass);
        Task UploadBlobData(string ConnectionString);
    }
}