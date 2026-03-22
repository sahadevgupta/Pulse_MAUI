using Pulse_MAUI.Models;

namespace Pulse_MAUI.Interfaces
{
    public interface IItemService
    {
        Task<IEnumerable<Models.Database.Item>> GetItemListAsync();
        Task SaveItem(Models.Database.Item itemToSave);
    }
}