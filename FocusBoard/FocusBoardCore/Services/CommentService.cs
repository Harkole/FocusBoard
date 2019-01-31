using FocusBoardCore.Interfaces;
using FocusBoardCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FocusBoardCore.Services
{
    public class CommentService : ICommentService
    {
        protected readonly ICommentRepository repository;

        public CommentService(ICommentRepository commentRepository)
        {
            repository = commentRepository ?? throw new ArgumentNullException(nameof(commentRepository));
        }


        public Task<Comment> CreateNewCommentAsync(Comment comment, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }


        public Task DeleteCommentAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }


        public Task<IEnumerable<Comment>> GetCommentByAuthor(string authorId, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }


        public Task<IEnumerable<Comment>> GetCommentsByItem(string itemId, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }


        public Task<IEnumerable<Comment>> GetCommentsByVotes(int minVoteCount, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }


        public Task<Comment> UpdateCommentAsync(Comment comment, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }
}
