using FocusBoardCore.Interfaces;
using FocusBoardCore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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

        /// <summary>
        /// Remove an item from the system
        /// </summary>
        /// <param name="Id">The identity of the item to remove</param>
        /// <param name="cancellationToken"></param>
        public async Task DeleteItemAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            // Validate the id isn't empty
            if (!string.IsNullOrEmpty(id))
            {
                await repository.DeleteItemAsync(id, cancellationToken);
            }
            else
            {
                throw new ArgumentNullException(nameof(id), "The identity must be a value");
            }
        }

        /// <summary>
        /// Returns a list of all Items in the system
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>An IEnumerable<Item> collection of all Items</returns>
        public async Task<IEnumerable<Item>> GetAllItemsAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await repository.GetAllItemsAsync(cancellationToken);
        }

        /// <summary>
        /// Returns a single item identified by its system identity
        /// </summary>
        /// <param name="id">The identity of the item to return</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The targetted Item object</returns>
        public async Task<Item> GetItemByIdAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!string.IsNullOrEmpty(id))
            {
                return await repository.GetItemByIdAsync(id, cancellationToken);
            }
            else
            {
                throw new ArgumentNullException(nameof(id), "The identity value must be provided");
            }
        }

        /// <summary>
        /// Returns the list of items created by a targetted Author Identity
        /// </summary>
        /// <param name="authorId">The Author Identity to target</param>
        /// <param name="cancellationToken"></param>
        /// <returns>An IEnumerable<Item> of Items created by the Author specified</returns>
        public async Task<IEnumerable<Item>> GetItemsByAuthorAsync(string authorId, CancellationToken cancellationToken = default(CancellationToken))
        {
            IEnumerable<Item> items;

            if (!string.IsNullOrEmpty(authorId))
            {
                items = await repository.GetItemsByAuthorAsync(authorId, cancellationToken);

                if (0 == items.Count())
                {
                    // Nothing was found, null the value
                    items = null;
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(authorId), "The Author Identity value must be provided");
            }

            return items;
        }

        /// <summary>
        /// Updates all values associated with the supplied Item object
        /// </summary>
        /// <param name="item">The new Object values, including the current Item Identity</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The update Item object as held in the repository</returns>
        public async Task<Item> UpdateItemAsync(Item item, CancellationToken cancellationToken = default(CancellationToken))
        {
            // Validate the Model, the results will be collected in the ICollection, but if everything is OK then isValid = true
            ICollection<ValidationResult> validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(item, new ValidationContext(item), validationResults, true);

            if (isValid && !string.IsNullOrEmpty(item.Id))
            {
                return await repository.GetItemByIdAsync(await repository.UpdateItemAsync(item, cancellationToken), cancellationToken);
            }
            else
            {
                // First check the validation results
                foreach (ValidationResult vr in validationResults)
                {
                    throw new ArgumentNullException(((string[])vr.MemberNames)[0], vr.ErrorMessage);
                }

                // If there wasn't any ValidationResults, then the issue is a missing Identity value
                throw new ArgumentNullException(nameof(item.Id), "The Item Identity must be provided");
            }
        }
    }
}
