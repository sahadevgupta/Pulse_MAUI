using Pulse_MAUI.Models.Request;
using Pulse_MAUI.Models.Response;
using Refit;
using System.Text.Json;

namespace Pulse_MAUI.Interfaces
{
    public interface IProjectBackendService
    {
        #region [ GET Method ]

        [Get("/Utility/AppConfig")]
        Task<string> GetAppConfigAsync([HeaderCollection] IDictionary<string, string> headers);

        [Get("/Utility/AzureConnection")]
        Task<string> GetAzureConnectionAsync([HeaderCollection] IDictionary<string, string> headers);

        [Get("/userinfo")]
        Task<object> GetUserInfoAsync([HeaderCollection] IDictionary<string, string> headers);

        #endregion

        #region [ POST Methods ]

        [Post("/.auth/login/aad")] 
        Task<MobileServiceLoginDto> LoginAsync([Body(BodySerializationMethod.Serialized)] LoginRequest payload);

        [Post("/SyncLog")]
        Task PostSyncLogAsync([HeaderCollection] IDictionary<string, string> headers,[Body(BodySerializationMethod.Serialized)] SyncLogRequest request);

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
