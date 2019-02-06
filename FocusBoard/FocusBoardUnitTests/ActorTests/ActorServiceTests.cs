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

namespace FocusBoardUnitTests
{
    public class ActorServiceTests
    {
        protected IActorService ServiceUnderTest { get; }
        protected Mock<IActorRepository> ActorRepositoryMock { get; }

        // Repository Data Mock up
        protected ReadOnlyCollection<Actor> MockDataItems { get; } = new ReadOnlyCollection<Actor>(
            new List<Actor> {
                new Actor { Id = Guid.NewGuid().ToString(), Email = "anderson@matrix.com", Alias = "Neo", Hidden = false  },
                new Actor { Id = Guid.NewGuid().ToString(), Email = "agent.smith@matrix.com", Alias = "Smith", Hidden = true },
                new Actor { Id = Guid.NewGuid().ToString(), Email = "jane.doe@server.com", Alias = "Bambi", Hidden = false  },
                new Actor { Id = Guid.NewGuid().ToString(), Email = "bob.dave@server.com", Alias = "MusicMan", Hidden = false  },
                new Actor { Id = Guid.NewGuid().ToString(), Email = "matt.earl@email.com", Alias = "Freelancer", Hidden = false  },
                new Actor { Id = Guid.NewGuid().ToString(), Email = "l.jiffy@email.com", Alias = "Speedy", Hidden = true },
            }
        );

        public ActorServiceTests()
        {
            ActorRepositoryMock = new Mock<IActorRepository>();
            ServiceUnderTest = new ActorService(ActorRepositoryMock.Object);
        }

        #region Create

        [Fact]
        public async Task Should_create_new_actor()
        {
            // Arrange            
            string id = Guid.NewGuid().ToString();
            Actor newActor = new Actor { Alias = "Nikita", Email = "secret.agent@email.com", Hidden = true, Id = id };

            ActorRepositoryMock.Setup(x => x.CreateNewUserAsync(newActor, default(CancellationToken))).ReturnsAsync(id);
            ActorRepositoryMock.Setup(x => x.GetActorByIdAsync(id, default(CancellationToken))).ReturnsAsync(newActor);

            // Act
            Actor resultActor = await ServiceUnderTest.CreateNewUserAsync(newActor, default(CancellationToken));

            // Assert
            Assert.Equal(newActor, resultActor);
        }

        [Fact]
        public async Task Should_fail_create_new_actor_missing_email()
        {
            // Arrange
            Actor newActor = new Actor { Id = Guid.NewGuid().ToString(), Alias = "Tweedle" };
            ActorRepositoryMock.Setup(x => x.CreateNewUserAsync(newActor, default(CancellationToken))).ReturnsAsync(newActor.Id);
            ActorRepositoryMock.Setup(x => x.GetActorByIdAsync(newActor.Id, default(CancellationToken))).ReturnsAsync(newActor);

            // Act
            ArgumentNullException exception = await Assert.ThrowsAsync<ArgumentNullException>(() => ServiceUnderTest.CreateNewUserAsync(newActor, default(CancellationToken)));

            // Assert
            Assert.Equal(nameof(newActor.Email), exception.ParamName);
        }

        #endregion

        #region Read

        [Fact]
        public async Task Should_get_actor_by_id()
        {
            // Arrange
            string id = MockDataItems[1].Id;
            Actor expectedActor = MockDataItems[1];

            ActorRepositoryMock.Setup(x => x.GetActorByIdAsync(id, default(CancellationToken))).ReturnsAsync(MockDataItems.FirstOrDefault(i => i.Id == id));

            // Act
            Actor resultActor = await ServiceUnderTest.GetUserByIdAsync(id, default(CancellationToken));

            // Assert
            Assert.Equal(expectedActor, resultActor);

        }

        [Fact]
        public async Task Should_fail_get_actor_by_id_return_null_when_not_found()
        {
            // Arrange
            string id = Guid.NewGuid().ToString();
            ActorRepositoryMock.Setup(x => x.GetActorByIdAsync(id, default(CancellationToken))).ReturnsAsync(MockDataItems.FirstOrDefault(i => i.Id == id));

            // Act
            Actor resultActor = await ServiceUnderTest.GetUserByIdAsync(id, default(CancellationToken));

            // Assert
            Assert.Null(resultActor);
        }

        [Fact]
        public async Task Should_fail_get_actor_by_id_missing_id()
        {
            // Arrange
            string id = string.Empty;
            ActorRepositoryMock.Setup(x => x.GetActorByIdAsync(id, default(CancellationToken))).ReturnsAsync(MockDataItems.FirstOrDefault(i => i.Id == id));

            // Act
            ArgumentNullException exception = await Assert.ThrowsAsync<ArgumentNullException>(() => ServiceUnderTest.GetUserByIdAsync(id, default(CancellationToken)));

            // Assert
            Assert.Equal(nameof(id), exception.ParamName);
        }

        [Fact]
        public async Task Should_get_actor_by_email()
        {
            // Arrange
            Actor exepctedActor = MockDataItems[1];
            string email = exepctedActor.Email;

            ActorRepositoryMock.Setup(x => x.GetActorByEmailAsync(email, default(CancellationToken))).ReturnsAsync(MockDataItems.FirstOrDefault(i => i.Email == email));

            // Act
            Actor resultActor = await ServiceUnderTest.GetUserByEmailAsync(email, default(CancellationToken));

            // Assert
            Assert.Equal(exepctedActor, resultActor);
        }

        [Fact]
        public async Task Should_fail_get_actor_by_email_return_null_when_not_found()
        {
            // Arrange
            string email = "no.email@here.com";
            ActorRepositoryMock.Setup(x => x.GetActorByEmailAsync(email, default(CancellationToken))).ReturnsAsync(MockDataItems.FirstOrDefault(i => i.Email == email));

            // Act
            Actor resultActor = await ServiceUnderTest.GetUserByEmailAsync(email, default(CancellationToken));

            // Assert
            Assert.Null(resultActor);
        }

        [Fact]
        public async Task Should_fail_get_actor_by_email_missing_email()
        {
            // Arrange
            string email = string.Empty;
            ActorRepositoryMock.Setup(x => x.GetActorByEmailAsync(email, default(CancellationToken))).ReturnsAsync(MockDataItems.FirstOrDefault(i => i.Email == email));

            // Act
            ArgumentNullException exception = await Assert.ThrowsAsync<ArgumentNullException>(() => ServiceUnderTest.GetUserByEmailAsync(email, default(CancellationToken)));

            // Assert
            Assert.Equal(nameof(email), exception.ParamName);
        }

        #endregion

        #region Update

        [Fact]
        public async Task Should_update_actor()
        {
            // Arrange
            Actor newActor = new Actor { Id = MockDataItems[2].Id, Email = "new.dawn@emails.com" };

            ActorRepositoryMock.Setup(x => x.UpdateActorAsync(newActor, default(CancellationToken))).ReturnsAsync(newActor.Id);
            ActorRepositoryMock.Setup(x => x.GetActorByIdAsync(newActor.Id, default(CancellationToken))).ReturnsAsync(newActor);

            // Act
            Actor resultActor = await ServiceUnderTest.UpdateUserAsync(newActor, default(CancellationToken));

            // Assert
            Assert.Equal(newActor, resultActor);
        }

        [Fact]
        public async Task Should_fail_update_actor_no_id()
        {
            // Arrange
            Actor newActor = new Actor { Email = "info.address@server.com" };
            ActorRepositoryMock.Setup(x => x.UpdateActorAsync(newActor, default(CancellationToken))).ReturnsAsync(Guid.NewGuid().ToString());

            // Act
            ArgumentNullException exception = await Assert.ThrowsAsync<ArgumentNullException>(() => ServiceUnderTest.UpdateUserAsync(newActor, default(CancellationToken)));

            // Assert
            Assert.Equal(nameof(newActor.Id), exception.ParamName);
        }

        [Fact]
        public async Task Should_fail_update_actor_no_email()
        {
            // Arrange
            Actor newActor = new Actor { Id = MockDataItems[0].Id, Alias = "AllSeeing" };

            ActorRepositoryMock.Setup(x => x.UpdateActorAsync(newActor, default(CancellationToken))).ReturnsAsync(newActor.Id);
            ActorRepositoryMock.Setup(x => x.GetActorByIdAsync(newActor.Id, default(CancellationToken))).ReturnsAsync(newActor);

            // Act
            ArgumentNullException exception = await Assert.ThrowsAsync<ArgumentNullException>(() => ServiceUnderTest.UpdateUserAsync(newActor, default(CancellationToken)));

            // Assert
            Assert.Equal(nameof(newActor.Email), exception.ParamName);
        }

        #endregion

        #region Delete

        [Fact]
        public async Task Should_fail_delete_missing_id()
        {
            // Arrange
            string id = string.Empty;

            // Act
            ArgumentNullException exception = await Assert.ThrowsAsync<ArgumentNullException>(() => ServiceUnderTest.DeleteUserAsync(id, default(CancellationToken)));

            // Assert
            Assert.Equal(nameof(id), exception.ParamName);
        }

        #endregion
    }
}
