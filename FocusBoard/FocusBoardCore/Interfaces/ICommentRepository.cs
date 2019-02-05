using FocusBoardCore.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FocusBoardCore.Interfaces
{
    public interface ICommentRepository
    {

        Task<string> CreateNewCommentAsync(Comment comment, CancellationToken cancellationToken = default(CancellationToken));

        Task<Comment> GetCommentById(string id, CancellationToken cancellationToken = default(CancellationToken));
        Task<IEnumerable<Comment>> GetCommentsByParentAsync(string parentId, CancellationToken cancellationToken = default(CancellationToken));
        Task<IEnumerable<Comment>> GetCommentByAuthorAsync(string authorId, CancellationToken cancellationToken = default(CancellationToken));
        Task<IEnumerable<Comment>> GetCommentsByVotesAsync(int minVoteCount, CancellationToken cancellationToken = default(CancellationToken));

        Task<string> UpdateCommentAsync(Comment comment, CancellationToken cancellationToken = default(CancellationToken));

        Task DeleteCommentAsync(string id, CancellationToken cancellationToken = default(CancellationToken));
    }
}
