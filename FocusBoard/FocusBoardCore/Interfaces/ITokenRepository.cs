using FocusBoardCore.Models;
using System.Threading;
using System.Threading.Tasks;

namespace FocusBoardCore.Interfaces
{
    public interface ITokenRepository
    {
        Task<Authentication> GetClaimsValuesAsync(ActorLogin actor, CancellationToken cancellationToken = default(CancellationToken));
    }
}
