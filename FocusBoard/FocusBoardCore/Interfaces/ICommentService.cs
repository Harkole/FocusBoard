using FocusBoardCore.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FocusBoardCore.Interfaces
{
    public interface ICommentService
    {
        Task<Comment> CreateNewCommentAsync(Comment comment, CancellationToken cancellationToken = default(CancellationToken));

        Task<IEnumerable<Comment>> GetCommentsByItem(string itemId, CancellationToken cancellationToken = default(CancellationToken));
        Task<IEnumerable<Comment>> GetCommentByAuthor(string authorId, CancellationToken cancellationToken = default(CancellationToken));
        Task<IEnumerable<Comment>> GetCommentsByVotes(int minVoteCount, CancellationToken cancellationToken = default(CancellationToken));

        Task<Comment> UpdateCommentAsync(Comment comment, CancellationToken cancellationToken = default(CancellationToken));

        Task DeleteCommentAsync(string id, CancellationToken cancellationToken = default(CancellationToken));
    }
}
