using FocusBoardCore.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FocusBoardCore.Interfaces
{
    public interface IItemService
    {
        // Create
        Task<Item> CreateItemAsync(Item item, CancellationToken cancellationToken = default(CancellationToken));

        // Read
        Task<IEnumerable<Item>> GetAllItemsAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task<IEnumerable<Item>> GetItemsByAuthorAsync(string authorId, CancellationToken cancellationToken = default(CancellationToken));
        Task<Item> GetItemByIdAsync(string Id, CancellationToken cancellationToken = default(CancellationToken));

        // Update
        Task UpdateItemAsync(Item item, CancellationToken cancellationToken = default(CancellationToken));

        // Delete
        Task DeleteItemAsync(string Id, CancellationToken cancellationToken = default(CancellationToken));
    }
}
