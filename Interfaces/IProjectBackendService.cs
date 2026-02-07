using Pulse_MAUI.Models.Request;
using Refit;
using System.Text.Json;

namespace Pulse_MAUI.Interfaces
{
    public interface IProjectBackendService
    {
        #region [ GET Method ]

        [Get("/Utility/AppConfig")]
        Task<object> GetAppConfigAsync();

        [Get("/Utility/AzureConnection")]
        Task<object> GetAzureConnectionAsync();

        [Get("/userinfo")]
        Task<object> GetUserInfoAsync([HeaderCollection] IDictionary<string, string> headers);

        #endregion

        #region [ POST Methods ]

        [Post("/SyncLog")]
        Task<object> PostSyncLogAsync([Body(BodySerializationMethod.Serialized)] SyncLogRequest request);

        [Post("/api/{tableName}")]
        Task<JsonDocument> InsertAsync(string tableName, [Body] JsonDocument item);

        #endregion

        #region [ PUT Method ]

        [Put("/api/{tableName}/{id}")]
        Task<JsonDocument> UpdateAsync(string tableName, string id, [Body] JsonDocument item);

        #endregion

        #region [ DELETE Method ]

        [Delete("/api/{tableName}/{id}")]
        Task DeleteAsync(string tableName, string id);

        #endregion
    }

}
