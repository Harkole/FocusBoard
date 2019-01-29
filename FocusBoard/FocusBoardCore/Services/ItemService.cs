using FocusBoardCore.Interfaces;
using FocusBoardCore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        /// <exception cref="ArgumentNullException">Thrown when Dependancy Injection fails</exception>
        public ItemService(IItemRepository itemRepository)
        {
            repository= itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
        }

        /// <summary>
        /// Creates a new Item in the database after validating the data is correct
        /// </summary>
        /// <param name="item">The Item object to create</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The newly created Item object from the repository</returns>
        /// <exception cref="ArgumentNullException">Caused by invalid model state, the error will hold a message for the first invalid value, but there may be others</exception>
        public async Task<Item> CreateItemAsync(Item item, CancellationToken cancellationToken = default(CancellationToken))
        {
            Item createdItem = null;

            // Validate the Model, the results will be collected in the ICollection, but if everything is OK then isValid = true
            ICollection<ValidationResult> validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(item, new ValidationContext(item), validationResults, true);

            if (isValid)
            {
                // Create the new Item
                string id = await repository.CreateNewItemAsync(item, cancellationToken);

                // Get the item back as it appears in the repository
                createdItem = await repository.GetItemByIdAsync(id, cancellationToken);
            }
            else
            {
                foreach(ValidationResult vr in validationResults)
                {
                    throw new ArgumentNullException(((string[])vr.MemberNames)[0], vr.ErrorMessage);
                }
            }

            // Return the newly created item
            return createdItem;
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


        public Task<Item> UpdateItemAsync(Item item, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }
}
