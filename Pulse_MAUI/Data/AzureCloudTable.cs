using Microsoft.Datasync.Client;
using Pulse_MAUI.Abstractions;

namespace Pulse_MAUI.Data
{
	public class AzureCloudTable<T> : ICloudTable<T> where T : TableData
	{
        private readonly IOfflineTable<T> table;

        public AzureCloudTable(DatasyncClient client)
		{
			this.table = client.GetOfflineTable<T>();
		}

		#region ICloudTable implementation
		public async Task<T> CreateItemAsync(T item)
		{
			await table.InsertItemAsync(item);
			return item;
		}

		public async Task DeleteItemAsync(T item)
		{
			await table.DeleteItemAsync(item);
		}

		public async Task<ICollection<T>> ReadAllItemsAsync()
		{
			return await table.ToListAsync();
		}

		public async Task<T> ReadItemAsync(string id)
		{
            return await table.Where(x => x.Id == id)
                              .ToAsyncEnumerable()
							  .FirstOrDefaultAsync();
        }

		public async Task<T> UpdateItemAsync(T item)
		{
			await table.ReplaceItemAsync(item);
			return item;
		}

		public async Task PullAsync()
		{
			string queryName = $"incsync_{typeof(T).Name}";
			await table.PullItemsAsync(queryName);
		}

        public Task<T> UpsertItemAsync(T item)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<T>> ReadItemsAsync(int start, int count)
        {
            throw new NotImplementedException();
        }



        #endregion
    }
}
