using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pulse_MAUI.Services
{
    public class ItemService(IDataManager dataManager, IUserService userService) : IItemService
    {
        /// <summary>
		/// Get the values from the look up table async.
		/// </summary>
		/// <returns>The lookup list async.</returns>
		public async Task<IEnumerable<Item>> GetItemListAsync()
        {
            return await dataManager
                .GetAllItemsAsync();
        }

        /// <summary>
		/// Saves a list of Items.
		/// </summary>
		/// <returns>Task.</returns>
		/// <param name="itemsToSave">Activity Tasks to save.</param>
		public async Task SaveItem(Item itemToSave)
        {

            itemToSave.CreatedAt = DateTime.Now;

            //TODO: Need to check
            //itemToSave.UploadedBy = UserService.Instance.CurrentUser.ApexId;

            await dataManager.SaveItemAsync(itemToSave);


        }
    }
}
