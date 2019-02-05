using FocusBoardCore.Interfaces;
using FocusBoardCore.Models;
using FocusBoardCore.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace FocusBoardUnitTests.CommentTests
{
    public class CommentServiceTests
    {
        protected ICommentService ServiceUnderTest { get; }
        protected Mock<ICommentRepository> CommentRepositoryMock { get; }

        // Repository Data Mock up
        protected ReadOnlyCollection<Comment> MockDataItems { get; } = new ReadOnlyCollection<Comment>(
            new List<Comment> {
                new Comment { Id = Guid.NewGuid().ToString(), AuthorId = Guid.NewGuid().ToString(), ParentId = Guid.NewGuid().ToString(), Value = "I really like this idea!", Votes = 1 },
                new Comment { Id = Guid.NewGuid().ToString(), AuthorId = Guid.NewGuid().ToString(), ParentId = Guid.NewGuid().ToString(), Value = "You have no idea what you're doing", Votes = -2 },
                new Comment { Id = Guid.NewGuid().ToString(), AuthorId = Guid.NewGuid().ToString(), ParentId = Guid.NewGuid().ToString(), Value = "I think it needs to have 2 extra phone number fields as well though", Votes = 5 },
                new Comment { Id = Guid.NewGuid().ToString(), AuthorId = Guid.NewGuid().ToString(), ParentId = Guid.NewGuid().ToString(), Value = "Could we keep the images?", Votes = 4 },
                new Comment { Id = Guid.NewGuid().ToString(), AuthorId = Guid.NewGuid().ToString(), ParentId = Guid.NewGuid().ToString(), Value = "The search engine needs to be less flexible to provide better answers, and certainly quicker", Votes = 10 },
                new Comment { Id = Guid.NewGuid().ToString(), AuthorId = Guid.NewGuid().ToString(), ParentId = Guid.NewGuid().ToString(), Value = "Don't worry about losing the duplicate data in there", Votes = 11 },
            }
        );

        public CommentServiceTests()
        {
            CommentRepositoryMock = new Mock<ICommentRepository>();
            ServiceUnderTest = new CommentService(CommentRepositoryMock.Object);
        }

        #region Create

        [Fact]
        public async Task Should_create_new_comment()
        {
            // Arrange
            string newId = Guid.NewGuid().ToString();
            Comment newComment = new Comment { Id = newId, AuthorId = Guid.NewGuid().ToString(), ParentId = Guid.NewGuid().ToString(), Value = "This new comment is awesome", Votes = 0 };
            
            CommentRepositoryMock.Setup(x => x.CreateNewCommentAsync(newComment, default(CancellationToken))).ReturnsAsync(newId);
            CommentRepositoryMock.Setup(x => x.GetCommentById(newId, default(CancellationToken))).ReturnsAsync(newComment);

            // Act
            Comment resultComment = await ServiceUnderTest.CreateNewCommentAsync(newComment, default(CancellationToken));

            // Assert
            Assert.Same(newComment, resultComment);
        }

        [Fact]
        public async Task Should_fail_create_new_comment_no_parent()
        {
            // Arrange
            Comment newComment = new Comment { AuthorId = Guid.NewGuid().ToString(), Value = "This new comment is awesome", Votes = 0 };
            CommentRepositoryMock.Setup(x => x.CreateNewCommentAsync(newComment, default(CancellationToken))).ReturnsAsync(Guid.NewGuid().ToString());

            // Act
            ArgumentNullException exception = await Assert.ThrowsAsync<ArgumentNullException>(() => ServiceUnderTest.CreateNewCommentAsync(newComment, default(CancellationToken)));

            // Assert
            Assert.Equal(nameof(newComment.ParentId), exception.ParamName);

        }

        [Fact]
        public async Task Should_fail_create_new_comment_no_author_id()
        {
            // Arrange
            Comment newComment = new Comment { ParentId = Guid.NewGuid().ToString(), Value = "This new comment is awesome", Votes = 0 };
            CommentRepositoryMock.Setup(x => x.CreateNewCommentAsync(newComment, default(CancellationToken))).ReturnsAsync(Guid.NewGuid().ToString());

            // Act
            ArgumentNullException exception = await Assert.ThrowsAsync<ArgumentNullException>(() => ServiceUnderTest.CreateNewCommentAsync(newComment, default(CancellationToken)));

            // Assert
            Assert.Equal(nameof(newComment.AuthorId), exception.ParamName);
        }

        [Fact]
        public async Task Should_fail_create_new_comment_no_value()
        {
            // Arrange
            Comment newComment = new Comment { ParentId = Guid.NewGuid().ToString(), AuthorId = Guid.NewGuid().ToString(), Votes = 0 };
            CommentRepositoryMock.Setup(x => x.CreateNewCommentAsync(newComment, default(CancellationToken))).ReturnsAsync(Guid.NewGuid().ToString());

            // Act
            ArgumentNullException exception = await Assert.ThrowsAsync<ArgumentNullException>(() => ServiceUnderTest.CreateNewCommentAsync(newComment, default(CancellationToken)));

            // Assert
            Assert.Equal(nameof(newComment.Value), exception.ParamName);
        }

        #endregion

        #region Read
        [Fact]
        public async Task Should_return_items_by_parent()
        {
            // Arrange
            string parentId = MockDataItems[2].ParentId;
            CommentRepositoryMock.Setup(x => x.GetCommentsByParentAsync(parentId, default(CancellationToken))).ReturnsAsync(MockDataItems.Where(v => v.ParentId == parentId));

            // Act
            IEnumerable<Comment> resultComments = await ServiceUnderTest.GetCommentsByParentAsync(parentId, default(CancellationToken));

            // Assert
            Assert.Equal(MockDataItems.Where(v => v.ParentId == parentId), resultComments);
        }

        [Fact]
        public async Task Should_return_null_when_parent_not_found()
        {
            // Arrange
            string parentId = Guid.NewGuid().ToString();
            CommentRepositoryMock.Setup(x => x.GetCommentsByParentAsync(parentId, default(CancellationToken))).ReturnsAsync(MockDataItems.Where(v => v.ParentId == parentId));

            // Act
            IEnumerable<Comment> resultComments = await ServiceUnderTest.GetCommentsByParentAsync(parentId, default(CancellationToken));

            // Assert
            Assert.Empty(resultComments);

        }

        [Fact]
        public async Task Should_fail_to_return_by_parent_no_value()
        {

            // Arrange
            string parentId = string.Empty;

            // Act
            ArgumentNullException exception = await Assert.ThrowsAsync<ArgumentNullException>(() => ServiceUnderTest.GetCommentsByParentAsync(parentId, default(CancellationToken)));

            // Assert
            Assert.Equal(nameof(parentId), exception.ParamName);
        }

        [Fact]
        public async Task Should_return_items_by_author()
        {
            // Arrange
            string authorId = MockDataItems[1].AuthorId;
            CommentRepositoryMock.Setup(x => x.GetCommentByAuthorAsync(authorId, default(CancellationToken))).ReturnsAsync(MockDataItems.Where(i => i.AuthorId == authorId));

            IEnumerable<Comment> expectedData = MockDataItems.Where(i => i.AuthorId == authorId);

            // Act
            IEnumerable<Comment> resultComments = await ServiceUnderTest.GetCommentByAuthorAsync(authorId, default(CancellationToken));

            // Assert
            Assert.Equal(expectedData, resultComments);
        }

        [Fact]
        public async Task Should_return_null_when_author_not_found()
        {
            // Arrange
            string authorId = Guid.NewGuid().ToString();
            CommentRepositoryMock.Setup(x => x.GetCommentByAuthorAsync(authorId, default(CancellationToken))).ReturnsAsync(MockDataItems.Where(v => v.AuthorId == authorId));

            // Act
            IEnumerable<Comment> resultComments = await ServiceUnderTest.GetCommentByAuthorAsync(authorId, default(CancellationToken));

            // Assert
            Assert.Empty(resultComments);
        }
        
        #endregion

        #region Update

        [Fact]
        public async Task Should_update_existing_comment()
        {
            // Arrange
            Comment newComment = new Comment { Id = Guid.NewGuid().ToString(), AuthorId = Guid.NewGuid().ToString(), ParentId = Guid.NewGuid().ToString(), Value = "I'm prone to changing my mind, maybe more than once", Votes = 0 };

            CommentRepositoryMock.Setup(x => x.UpdateCommentAsync(newComment, default(CancellationToken))).ReturnsAsync(newComment.Id);
            CommentRepositoryMock.Setup(x => x.GetCommentById(newComment.Id, default(CancellationToken))).ReturnsAsync(newComment);

            // Act
            Comment resultComment = await ServiceUnderTest.UpdateCommentAsync(newComment, default(CancellationToken));

            // Assert
            Assert.Equal(newComment, resultComment);
        }

        [Fact]
        public async Task Should_fail_update_no_parent()
        {
            // Arrange
            Comment newComment = new Comment { AuthorId = Guid.NewGuid().ToString(), Value = "This new comment is awesome", Votes = 0 };
            CommentRepositoryMock.Setup(x => x.UpdateCommentAsync(newComment, default(CancellationToken))).ReturnsAsync(Guid.NewGuid().ToString());

            // Act
            ArgumentNullException exception = await Assert.ThrowsAsync<ArgumentNullException>(() => ServiceUnderTest.CreateNewCommentAsync(newComment, default(CancellationToken)));

            // Assert
            Assert.Equal(nameof(newComment.ParentId), exception.ParamName);

        }

        [Fact]
        public async Task Should_fail_update_no_author_id()
        {
            // Arrange
            Comment newComment = new Comment { ParentId = Guid.NewGuid().ToString(), Value = "This new comment is awesome", Votes = 0 };
            CommentRepositoryMock.Setup(x => x.UpdateCommentAsync(newComment, default(CancellationToken))).ReturnsAsync(Guid.NewGuid().ToString());

            // Act
            ArgumentNullException exception = await Assert.ThrowsAsync<ArgumentNullException>(() => ServiceUnderTest.CreateNewCommentAsync(newComment, default(CancellationToken)));

            // Assert
            Assert.Equal(nameof(newComment.AuthorId), exception.ParamName);
        }

        [Fact]
        public async Task Should_fail_update_no_value()
        {
            // Arrange
            Comment newComment = new Comment { ParentId = Guid.NewGuid().ToString(), AuthorId = Guid.NewGuid().ToString(), Votes = 0 };
            CommentRepositoryMock.Setup(x => x.UpdateCommentAsync(newComment, default(CancellationToken))).ReturnsAsync(Guid.NewGuid().ToString());

            // Act
            ArgumentNullException exception = await Assert.ThrowsAsync<ArgumentNullException>(() => ServiceUnderTest.CreateNewCommentAsync(newComment, default(CancellationToken)));

            // Assert
            Assert.Equal(nameof(newComment.Value), exception.ParamName);
        }
        
        #endregion

        #region Delete

        [Fact]
        public async Task Should_fail_delete_missing_id()
        {
            // Arrange
            string id = string.Empty;

            // Act
            ArgumentNullException exception = await Assert.ThrowsAsync<ArgumentNullException>(() => ServiceUnderTest.DeleteCommentAsync(id, default(CancellationToken)));

            // Assert
            Assert.Equal(nameof(id), exception.ParamName);
        }
        
        #endregion
    }
}
