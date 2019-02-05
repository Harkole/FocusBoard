using FocusBoardCore.Models;
using System.Threading;
using System.Threading.Tasks;

namespace FocusBoardCore.Interfaces
{
    public interface IActorRepository
    {
        Task<string> CreateNewUserAsync(Actor actor, CancellationToken cancellationToken = default(CancellationToken));

        Task<Actor> GetActorByIdAsync(string id, CancellationToken cancellationToken = default(CancellationToken));
        Task<Actor> GetActorByEmailAsync(string email, CancellationToken cancellationToken = default(CancellationToken));

        Task<string> UpdateActorAsync(Actor actor, CancellationToken cancellationToken = default(CancellationToken));

        Task DeleteUserAsync(string id, CancellationToken cancellationToken = default(CancellationToken));
    }
}
