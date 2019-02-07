using FocusBoardCore.Models;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace FocusBoardCore.Interfaces
{
    public interface ITokenService
    {
        Task<ActorToken> GetClaimsIdentityAsync(ActorLogin actor, CancellationToken cancellationToken = default(CancellationToken));

        ActorToken RenewClaimsIdentity(ClaimsPrincipal claims);
    }
}
