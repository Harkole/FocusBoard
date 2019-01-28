using FocusBoardCore.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FocusBoardCore.Interfaces
{
    public interface IItemRepository
    {
        Task<string> CreateNewItemAsync(Item item, CancellationToken cancellationToken = default(CancellationToken));

        Task<IEnumerable<Item>> GetAllItemsAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task<IEnumerable<Item>> GetItemsByAuthorAsync(string authorId, CancellationToken cancellationToken = default(CancellationToken));
        Task<Item> GetItemByIdAsync(string itemId, CancellationToken cancellationToken = default(CancellationToken));

        Task<string> UpdateItemAsync(Item item, CancellationToken cancellationToken = default(CancellationToken));

        Task DeleteItemAsync(string id, CancellationToken cancellationToken = default(CancellationToken));
    }
}
