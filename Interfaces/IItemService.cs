using Pulse_MAUI.Models;

namespace Pulse_MAUI.Interfaces
{
    public interface IItemService
    {
        Task<IEnumerable<Item>> GetItemListAsync();
        Task SaveItem(Item itemToSave);
    }
}