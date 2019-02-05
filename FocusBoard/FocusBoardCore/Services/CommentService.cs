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
    public class CommentService : ICommentService
    {
        protected readonly ICommentRepository repository;

        /// <summary>
        /// Provides the logic layer for all Comments
        /// </summary>
        /// <param name="commentRepository">The Comment Repository layer contract</param>
        public CommentService(ICommentRepository commentRepository)
        {
            repository = commentRepository ?? throw new ArgumentNullException(nameof(commentRepository));
        }

        /// <summary>
        /// Creates a new Comment in the system as long as the Author, Parent 
        /// and Value fields contain data
        /// </summary>
        /// <param name="comment">The Comment object to create</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The Comment object that was created in the Repository</returns>
        public async Task<Comment> CreateNewCommentAsync(Comment comment, CancellationToken cancellationToken = default(CancellationToken))
        {
            Comment createdComment = null;

            // Validate the Model, the results will be collected in the ICollection, but if everything is OK then isValid = true
            ICollection<ValidationResult> validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(comment, new ValidationContext(comment), validationResults, true);

            if (isValid)
            {
                // Create the new Item
                string id = await repository.CreateNewCommentAsync(comment, cancellationToken);

                // Get the item back as it appears in the repository
                createdComment = await repository.GetCommentById(id, cancellationToken);
            }
            else
            {
                foreach (ValidationResult vr in validationResults)
                {
                    throw new ArgumentNullException(((string[])vr.MemberNames)[0], vr.ErrorMessage);
                }
            }

            // Return the newly created item
            return createdComment;
        }

        /// <summary>
        /// Removes the specified Comment from the system
        /// </summary>
        /// <param name="id">The identity of the comment to remove</param>
        /// <param name="cancellationToken"></param>
        public async Task DeleteCommentAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            if(!string.IsNullOrEmpty(id))
            {
                await repository.DeleteCommentAsync(id, cancellationToken);
            }
            else
            {
                throw new ArgumentNullException(nameof(id), "The identity value must contain a value");
            }
        }

        /// <summary>
        /// Gets all comments by a particular Author
        /// </summary>
        /// <param name="authorId">The author identity to get comments for</param>
        /// <param name="cancellationToken"></param>
        /// <returns>All comments belonging to the targetted Author</returns>
        public async Task<IEnumerable<Comment>> GetCommentByAuthorAsync(string authorId, CancellationToken cancellationToken = default(CancellationToken))
        {
            IEnumerable<Comment> comments;

            if (!string.IsNullOrEmpty(authorId))
            {
                comments = await repository.GetCommentByAuthorAsync(authorId, cancellationToken);
            }
            else
            {
                throw new ArgumentNullException(nameof(authorId), "The author identity must be provided");
            }

            return comments;
        }

        /// <summary>
        /// Gets all comments by the Parent Identity (Usually the Item Identity)
        /// </summary>
        /// <param name="itemId">The Identity to match to the ParentId field</param>
        /// <param name="cancellationToken"></param>
        /// <returns>All comments belonging to the Parent Id specified</returns>
        public async Task<IEnumerable<Comment>> GetCommentsByParentAsync(string parentId, CancellationToken cancellationToken = default(CancellationToken))
        {
            IEnumerable<Comment> comments;

            if(!string.IsNullOrEmpty(parentId))
            {
                comments = await repository.GetCommentsByParentAsync(parentId, cancellationToken);
            }
            else
            {
                throw new ArgumentNullException(nameof(parentId), "The Parent Identity value must be supplied");
            }

            return comments;
        }

        /// <summary>
        /// Gets all Comments where the vote count is higher or equal to the supplied vote count
        /// </summary>
        /// <param name="minVoteCount">The lowest value to return</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A list of comments who's votes are equal to or higher than the supplied vote count</returns>
        public async Task<IEnumerable<Comment>> GetCommentsByVotesAsync(int minVoteCount, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await repository.GetCommentsByVotesAsync(minVoteCount, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comment"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Comment> UpdateCommentAsync(Comment comment, CancellationToken cancellationToken = default(CancellationToken))
        {
            // Validate the Model, the results will be collected in the ICollection, but if everything is OK then isValid = true
            ICollection<ValidationResult> validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(comment, new ValidationContext(comment), validationResults, true);

            if (isValid && !string.IsNullOrEmpty(comment.Id))
            {
                string updatedId = await repository.UpdateCommentAsync(comment, cancellationToken);
                return await repository.GetCommentById(updatedId, cancellationToken);
            }
            else
            {
                // First check the validation results
                foreach (ValidationResult vr in validationResults)
                {
                    throw new ArgumentNullException(((string[])vr.MemberNames)[0], vr.ErrorMessage);
                }

                // If there wasn't any ValidationResults, then the issue is a missing Identity value
                throw new ArgumentNullException(nameof(comment.Id), "The Item Identity must be provided");
            }
        }
    }
}
