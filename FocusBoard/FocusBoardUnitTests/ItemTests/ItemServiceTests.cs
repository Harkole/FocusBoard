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

namespace FocusBoardUnitTests.ItemTests
{
    public class ItemServiceTests
    {
        protected ItemService ServiceUnderTest { get; }
        protected Mock<IItemRepository> ItemRepositoryMock { get; }

        // Repository Data Mock up
        protected ReadOnlyCollection<Item> MockDataItems { get; } = new ReadOnlyCollection<Item>(
            new List<Item> {
                new Item { Id = Guid.NewGuid().ToString(), Author = "John Doe", HideAuthor = false, Title = "New Testing units", Description = "Too many bugs are getting through, do you guys even test?", Votes = -1 },
                new Item { Id = Guid.NewGuid().ToString(), Author = "Jane Doe", HideAuthor = true, Title = "Adding new items is too simple", Description = "I think it needs to have more clicks to add an item", Votes = -7 },
                new Item { Id = Guid.NewGuid().ToString(), Author = "Max Power", HideAuthor = false, Title = "Add search feature", Description = "It would be better if we could search existing items rather than duplicate things", Votes = 10 },
                new Item { Id = Guid.NewGuid().ToString(), Author = "Max Power", HideAuthor = false, Title = "Suggest Title based on program area", Description = "Have the Title suggest constructive input values", Votes = 4 },
                new Item { Id = Guid.NewGuid().ToString(), Author = "Xander Smith", HideAuthor = false, Title = "I need to find something", Description = "Can anyone help?", Votes = 0 },
            }
        );

        public ItemServiceTests()
        {
            ItemRepositoryMock = new Mock<IItemRepository>();
            ServiceUnderTest = new ItemService(ItemRepositoryMock.Object);
        }
        
        #region Get Items By Author

        [Fact]
        public async Task Should_return_items_by_author()
        {
            // Arrange
            string author = MockDataItems[2].Author;
            IEnumerable<Item> expectedData = MockDataItems.Where(i => i.Author == author);

            ItemRepositoryMock.Setup(x => x.GetItemsByAuthorAsync(author, default(CancellationToken))).ReturnsAsync(expectedData);

            // Act
            IEnumerable<Item> resultItems = await ServiceUnderTest.GetItemsByAuthorAsync(author, default(CancellationToken));

            // Assert
            Assert.Same(expectedData, resultItems);
            
        }

        [Fact]
        public async Task Should_return_null_when_no_author_found()
        {
            // Arrange
            string author = Guid.NewGuid().ToString();
            IEnumerable<Item> expectedData = MockDataItems.Where(i => i.Author == author);

            ItemRepositoryMock.Setup(x => x.GetItemsByAuthorAsync(author, default(CancellationToken))).ReturnsAsync(expectedData);

            // Act
            IEnumerable<Item> resultItems = await ServiceUnderTest.GetItemsByAuthorAsync(author, default(CancellationToken));

            // Assert
            Assert.Null(resultItems);
        }

        #endregion

        #region Get Items By Id

        [Fact]
        public async Task Should_return_single_item_by_identity()
        {
            // Arrange
            Item expectedResult = MockDataItems[1];
            string id = expectedResult.Id;

            ItemRepositoryMock.Setup(x => x.GetItemByIdAsync(id, default(CancellationToken))).ReturnsAsync(expectedResult);

            // Act
            Item resultItem = await ServiceUnderTest.GetItemByIdAsync(id, default(CancellationToken));

            // Assert
            Assert.Same(expectedResult, resultItem);
        }

        [Fact]
        public async Task Should_return_null_when_no_match()
        {
            // Arrange
            Item badResult = new Item { Id = Guid.NewGuid().ToString(), Author = "Bob Marley", HideAuthor = true, Title = "No women", Description = "No cry", Votes = 2 };
            ItemRepositoryMock.Setup(x => x.GetItemByIdAsync(badResult.Id, default(CancellationToken))).ReturnsAsync(MockDataItems.FirstOrDefault(i => i.Id == badResult.Id));

            // Act
            Item resultItem = await ServiceUnderTest.GetItemByIdAsync(badResult.Id, default(CancellationToken));

            // Assert
            Assert.Null(resultItem);
        }

        #endregion

        #region Create Item

        [Fact]
        public async Task Should_create_new_item()
        {
            // Arrange
            Item newItem = new Item { Id = Guid.NewGuid().ToString(), Author = "J. K. Rowling", HideAuthor = false, Title = "Fly Phising", Description = "I was wondering if you could tell if you have a book...", Votes = 0 };
            ItemRepositoryMock.Setup(x => x.CreateNewItemAsync(newItem, default(CancellationToken))).ReturnsAsync(newItem.Id);
            ItemRepositoryMock.Setup(x => x.GetItemByIdAsync(newItem.Id, default(CancellationToken))).ReturnsAsync(newItem);

            // Act
            Item resultItem = await ServiceUnderTest.CreateItemAsync(newItem, default(CancellationToken));

            // Assert
            Assert.Same(newItem, resultItem);
        }

        [Fact]
        public async Task Should_fail_to_create_new_item_missing_author()
        {
            // Arrange
            Item newItem = new Item { Id = Guid.NewGuid().ToString(), Author = string.Empty, HideAuthor = false, Title = "Fly Phising", Description = "I was wondering if you could tell if you have a book...", Votes = 0 };
            
            // Act & Assert
            ArgumentNullException exception = await Assert.ThrowsAsync<ArgumentNullException>(() => ServiceUnderTest.CreateItemAsync(newItem, default(CancellationToken)));
            Assert.Equal(nameof(newItem.Author), exception.ParamName);
        }

        [Fact]
        public async Task Should_fail_to_create_new_item_missing_title()
        {
            // Arrange
            Item newItem = new Item { Id = Guid.NewGuid().ToString(), Author = "H.G Wells", HideAuthor = false, Title = string.Empty, Description = "Aliens killed by common cold, isn't there an antivirus yet?", Votes = 0 };

            // Act & Assert
            ArgumentNullException exception = await Assert.ThrowsAsync<ArgumentNullException>(() => ServiceUnderTest.CreateItemAsync(newItem, default(CancellationToken)));
            Assert.Equal(nameof(newItem.Title), exception.ParamName);
        }

        [Fact]
        public async Task Should_fail_to_create_new_item_missing_description()
        {
            // Arrange
            Item newItem = new Item { Id = Guid.NewGuid().ToString(), Author = "Arthur Dole", HideAuthor = false, Title = "Fly Phising", Description = string.Empty, Votes = 0 };
            
            // Act & Assert
            ArgumentNullException exception = await Assert.ThrowsAsync<ArgumentNullException>(() => ServiceUnderTest.CreateItemAsync(newItem, default(CancellationToken)));
            Assert.Equal(nameof(newItem.Description), exception.ParamName);
        }

        #endregion

        #region Update Item

        [Fact]
        public async Task Should_update_an_existing_item()
        {
            // Arrange
            Item newItem = new Item { Id = Guid.NewGuid().ToString(), Author = "Jane Smith", HideAuthor = true, Title = "Adding new items is too simple", Description = "I think it needs to have more clicks to add an item", Votes = -7 };
            ItemRepositoryMock.Setup(x => x.UpdateItemAsync(newItem, default(CancellationToken))).ReturnsAsync(newItem.Id);
            ItemRepositoryMock.Setup(x => x.GetItemByIdAsync(newItem.Id, default(CancellationToken))).ReturnsAsync(newItem);

            // Act
            Item resultItem = await ServiceUnderTest.UpdateItemAsync(newItem, default(CancellationToken));

            // Assert
            Assert.Equal(newItem, resultItem);
        }

        [Fact]
        public async Task Should_fail_update_missing_identity()
        {
            // Arrange
            Item newItem = new Item { Id = string.Empty, Author = "Jane Smith", HideAuthor = true, Title = "Adding new items is too simple", Description = "I think it needs to have more clicks to add an item", Votes = -7 };
            ItemRepositoryMock.Setup(x => x.UpdateItemAsync(newItem, default(CancellationToken))).ReturnsAsync(newItem.Id);

            // Act
            ArgumentNullException exception = await Assert.ThrowsAsync<ArgumentNullException>(() => ServiceUnderTest.UpdateItemAsync(newItem, default(CancellationToken)));

            // Assert
            Assert.Equal(nameof(newItem.Id), exception.ParamName);
        }

        [Fact]
        public async Task Should_fail_update_missing_author()
        {
            // Arrange
            Item newItem = new Item { Id = MockDataItems[1].Id, HideAuthor = MockDataItems[1].HideAuthor, Author = string.Empty, Title = MockDataItems[1].Title, Description = MockDataItems[1].Description, Votes = MockDataItems[1].Votes };

            // Act & Assert
            ArgumentNullException exception = await Assert.ThrowsAsync<ArgumentNullException>(() => ServiceUnderTest.UpdateItemAsync(newItem, default(CancellationToken)));
            Assert.Equal(nameof(newItem.Author), exception.ParamName);
        }

        [Fact]
        public async Task Should_fail_update_missing_title()
        {
            // Arrange
            Item newItem = new Item { Id = MockDataItems[1].Id, HideAuthor = MockDataItems[1].HideAuthor, Author = MockDataItems[1].Author, Title = string.Empty, Description = MockDataItems[1].Description, Votes = MockDataItems[1].Votes };

            // Act & Assert
            ArgumentNullException exception = await Assert.ThrowsAsync<ArgumentNullException>(() => ServiceUnderTest.UpdateItemAsync(newItem, default(CancellationToken)));
            Assert.Equal(nameof(newItem.Title), exception.ParamName);
        }
        
        [Fact]
        public async Task Should_fail_update_missing_description()
        {
            // Arrange
            Item newItem = new Item { Id = MockDataItems[1].Id, HideAuthor = MockDataItems[1].HideAuthor, Author = MockDataItems[1].Author, Title = MockDataItems[1].Title, Description = string.Empty, Votes = MockDataItems[1].Votes };

            // Act & Assert
            ArgumentNullException exception = await Assert.ThrowsAsync<ArgumentNullException>(() => ServiceUnderTest.UpdateItemAsync(newItem, default(CancellationToken)));
            Assert.Equal(nameof(newItem.Description), exception.ParamName);
        }

        #endregion

        #region Delete Item

        [Fact]
        public async Task Should_fail_delete_missing_id()
        {
            // Arrange
            string id = string.Empty;

            // Act
            ArgumentNullException exception = await Assert.ThrowsAsync<ArgumentNullException>(() => ServiceUnderTest.DeleteItemAsync(id, default(CancellationToken)));

            // Assert
            Assert.Equal(nameof(id), exception.ParamName);
        }

        #endregion
    }
}
