using FocusBoardCore.Interfaces;
using FocusBoardCore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace FocusBoardCore.Services
{
    public class ActorService : IActorService
    {
        private readonly IActorRepository repository;

        /// <summary>
        /// Provides the Logic layer for the Actor endpoints
        /// </summary>
        /// <param name="actorRepository">The repository layer contract</param>
        public ActorService(IActorRepository actorRepository)
        {
            repository = actorRepository ?? throw new ArgumentNullException(nameof(actorRepository));
        }

        /// <summary>
        /// Creates a new "actor" on the system, these are usually end users, but
        /// maybe another application
        /// </summary>
        /// <param name="actor">The actor to create</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The newly created Actor object from the Repository</returns>
        public async Task<Actor> CreateNewUserAsync(Actor actor, CancellationToken cancellationToken = default(CancellationToken))
        {
            Actor createdActor = null;

            // Validate the Actor model
            ICollection<ValidationResult> validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(actor, new ValidationContext(actor), validationResults, true);

            if (isValid)
            {
                // Create the Actor
                string id = await repository.CreateNewUserAsync(actor, cancellationToken);

                // Get the new Actor back from the repository
                createdActor = await repository.GetActorByIdAsync(id, cancellationToken);
            }
            else
            {
                foreach(ValidationResult vr in validationResults)
                {
                    throw new ArgumentNullException(((string[])vr.MemberNames)[0], vr.ErrorMessage);
                }
            }

            // Return the Created result
            return createdActor;
        }

        /// <summary>
        /// Deletes an Actor from the system
        /// </summary>
        /// <param name="id">The identity of the Actor to remove</param>
        /// <param name="cancellationToken"></param>
        public async Task DeleteUserAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!string.IsNullOrEmpty(id))
            {
                await repository.DeleteUserAsync(id, cancellationToken);
            }
            else
            {
                throw new ArgumentNullException(nameof(id), "The Identity value must be provided");
            }
        }

        /// <summary>
        /// Gets an Actor by the email value held in the repository
        /// </summary>
        /// <param name="email">The full email address to look up</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The Actor that matched the email address</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<Actor> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default(CancellationToken))
        {
            Actor actor;

            if (!string.IsNullOrEmpty(email))
            {
                actor = await repository.GetActorByEmailAsync(email, cancellationToken);
            }
            else
            {
                throw new ArgumentNullException(nameof(email), "Email must be specified");
            }

            return actor;
        }

        /// <summary>
        /// Gets an Actor by the Identity held by the system
        /// </summary>
        /// <param name="id">The identity to look up</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The Actor with matching identity</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<Actor> GetUserByIdAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            Actor actor;

            if (!string.IsNullOrEmpty(id))
            {
                actor = await repository.GetActorByIdAsync(id, cancellationToken);
            }
            else
            {
                throw new ArgumentNullException(nameof(id), "The Identity must be provided");
            }

            return actor;
        }

        /// <summary>
        /// Updates an existing Actor with new values
        /// </summary>
        /// <param name="actor">The update values</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The newly updated Actor object</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<Actor> UpdateUserAsync(Actor actor, CancellationToken cancellationToken = default(CancellationToken))
        {
            Actor updatedActor = null;

            // Validate the Actor object
            ICollection<ValidationResult> validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(actor, new ValidationContext(actor), validationResults, true);

            if (isValid && !string.IsNullOrEmpty(actor.Id))
            {
                // Update the Actor and return the new object from the repository
                string id = await repository.UpdateActorAsync(actor, cancellationToken);
                updatedActor = await repository.GetActorByIdAsync(id, cancellationToken);
            }
            else
            {
                // Get the validation error(s)
                foreach(ValidationResult vr in validationResults)
                {
                    throw new ArgumentNullException(((string[])vr.MemberNames)[0], vr.ErrorMessage);
                }

                // Getting this far means the model was valid, but the ID was missing
                throw new ArgumentNullException(nameof(actor.Id), "The Identity was missing from the Actor and is required for updates");
            }

            // Return the updated Actor
            return updatedActor;
        }
    }
}
