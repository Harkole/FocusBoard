using FocusBoardCore.Interfaces;
using FocusBoardCore.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FocusBoardCore.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository repository;

        /// <summary>
        /// All service layer logic for Items
        /// </summary>
        /// <param name="itemService">The Repository layer contract</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ItemService(IItemRepository itemRepository)
        {
            repository= itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
        }


        public Task<Item> CreateItemAsync(Item item, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }


        public Task DeleteItemAsync(string Id, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }


        public Task<IEnumerable<Item>> GetAllItemsAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }


        public Task<Item> GetItemByIdAsync(string Id, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }


        public Task<IEnumerable<Item>> GetItemsByAuthorAsync(string authorId, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }


        public Task UpdateItemAsync(Item item, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }
}
