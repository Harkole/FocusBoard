using FocusBoardCore.Models;
using System.Threading;
using System.Threading.Tasks;

namespace FocusBoardCore.Interfaces
{
    public interface IActorService
    {
        Task<Actor> CreateNewUserAsync(Actor actor, CancellationToken cancellationToken = default(CancellationToken));

        Task<Actor> GetUserByIdAsync(string id, CancellationToken cancellationToken = default(CancellationToken));
        Task<Actor> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default(CancellationToken));

        Task<Actor> UpdateUserAsync(Actor actor, CancellationToken cancellationToken = default(CancellationToken));

        Task DeleteUserAsync(string id, CancellationToken cancellationToken = default(CancellationToken));
    }
}
