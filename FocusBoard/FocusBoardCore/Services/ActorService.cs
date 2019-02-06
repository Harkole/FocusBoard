using FocusBoardCore.Interfaces;
using FocusBoardCore.Models;
using System;
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

        public Task<Actor> CreateNewUserAsync(Actor actor, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task DeleteUserAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<Actor> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<Actor> GetUserByIdAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<Actor> UpdateUserAsync(Actor actor, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }
}
