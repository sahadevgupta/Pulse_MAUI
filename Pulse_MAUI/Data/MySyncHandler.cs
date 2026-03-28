using Pulse_MAUI.Interfaces;
using Refit;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace PCATablet.Core.Data
{
	/// <summary>
	/// This class handles mobile service sync issues. Specify here what happens when there are conflicts
	/// in the sync process.
	/// Fogbugz Case:
	/// Author: Manuel Dambrine
	/// Created: 29/03/2013
	/// </summary>
	public class MySyncHandler
    {

        private readonly IProjectBackendService _api;
        /// <summary>
        ///  Initializes a new instance of the <see cref="MySyncHandler"/> class.
        /// </summary>
        /// <param name="api"></param>
        public MySyncHandler(IProjectBackendService api)
        {
            this._api = api;
        }

        /// <summary>
        /// This method gets called on every table operation during the sync process.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="operationType"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<JsonDocument?> ExecuteTableOperationAsync(string tableName, string operationType, JsonDocument item)
        {
            JsonDocument? result = null;
            bool retry;

            do
            {
                retry = false;

                try
                {
                    Debug.WriteLine($"Execute Table Operation");
                    Debug.WriteLine($"Table: {tableName}");
                    Debug.WriteLine($"Operation Type: {operationType}");

                    switch (operationType)
                    {
                        case "Insert":
                            result = await _api.InsertAsync(tableName, item);
                            break;

                        case "Update":
                            var id = item.RootElement.GetProperty("id").GetString();
                            result = await _api.UpdateAsync(tableName, id!, item);
                            break;

                        case "Delete":
                            var deleteId = item.RootElement.GetProperty("id").GetString();
                            await _api.DeleteAsync(tableName, deleteId!);
                            break;

                        default:
                            throw new InvalidOperationException("Unsupported operation type");
                    }
                }
                catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.PreconditionFailed)
                {
                    // Handle concurrency conflict
                    var serverJson = JsonDocument.Parse(await ex.GetContentAsAsync<string>());
                    var version = serverJson.RootElement.GetProperty("version").GetString();
                    // Update local item version
                    retry = true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Exception: {ex.Message}");
                    retry = false;
                }

            } while (retry);

            return result;

        }

        /// <summary>
        /// Gets called when the push is complete.
        /// </summary>
        /// <param name="success"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public Task OnPushCompleteAsync(bool success, string[] errors)
        {
            if (!success)
            {
                foreach (var err in errors)
                {
                    Debug.WriteLine($"Push Error: {err}");
                }
            }

            return Task.CompletedTask;
        }


        /// <summary>
        /// This method gets called on every table operation during the sync process.
        /// </summary>
        /// <returns>The table operation async.</returns>
        /// <param name="operation">Operation.</param>
  //      public async Task<JObject> ExecuteTableOperationAsync(IMobileServiceTableOperation operation)
		//{
  //          MobileServiceInvalidOperationException error = null;
  //          JObject result = null;
		//	MobileServicePreconditionFailedException preconditionFailed = null;

  //          Debug.WriteLine("");
  //          Debug.WriteLine("");
  //          Debug.WriteLine("Execute Table Operation");
  //          Debug.WriteLine("Table" + operation.Table.TableName.ToString());
  //          Debug.WriteLine("Operation Type" + operation.Kind.ToString());
  //          Debug.WriteLine("");
  //          Debug.WriteLine("");

                        
  //          do
  //          {
                

		//		try
		//		{
		//			preconditionFailed = null; // reset the exception
		//			result = await operation.ExecuteAsync();
		//		}
  //              // handle a Mobile Service Conflit Exception
  //              catch (MobileServiceConflictException ex)
  //              {
  //                  error = ex;
  //              }
  //              // handle a Mobile Service precondition fail exception
  //              catch (MobileServicePreconditionFailedException ex)
  //              {
  //                  preconditionFailed = ex;
  //              }
  //              //handle a Mobile Invalid OperationException
  //              catch (MobileServiceInvalidOperationException ex)
  //              {
  //                  var code = ex.Response.StatusCode;
  //                  if (ex.Response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
  //                  {
  //                      string errorMessage = await ex.Response.Content.ReadAsStringAsync();
  //                      preconditionFailed = new MobileServicePreconditionFailedException(null, new JObject());
  //                  }
  //                  else
  //                  {
  //                      error = ex;
  //                  }


  //              }
  //              // catch any remaining Exceptions
  //              catch (Exception ex)
  //              {
  //                  var Exception = ex.Message;
  //                  preconditionFailed = new MobileServicePreconditionFailedException(null, new JObject());
  //              }


		//		if (preconditionFailed != null)
		//		{
		//			// swap the _version (using JObject’s here to make generic across all model’s)
		//			operation.Item[MobileServiceSystemColumns.Version] = preconditionFailed.Value[MobileServiceSystemColumns.Version];
		//		}

		//	} while (preconditionFailed != null); // keep on trying until it succeeds

		//	return result;
		//}

		/// <summary>
		/// Gets called when the push is complete.
		/// </summary>
		/// <returns>The push complete async.</returns>
		/// <param name="result">Result.</param>
		//public Task OnPushCompleteAsync(MobileServicePushCompletionResult result)
		//{
  //          if (result.Status != MobileServicePushStatus.Complete)
  //          {
  //             foreach(MobileServiceTableOperationError err in result.Errors)
  //              {
  //                  var error = err.RawResult;
  //              }
  //          }

		//	return Task.FromResult(0);
		//}
	}
}

