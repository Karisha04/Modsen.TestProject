using Microsoft.EntityFrameworkCore;
using Modsen.TestProject.DAL;
using Modsen.TestProject.DAL.Entities;
using Modsen.TestProject.DAL.Repositories;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Modsen.TestProject.Tests
{
    public class UnitTest1
    {
        private ProjectDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<ProjectDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;

            return new ProjectDbContext(options);
        }

        [Fact]
        public async Task AddAsync_ShouldAddNewEventToDatabase()
        {
            // Arrange
            using var context = CreateContext();
            var repository = new Repository<NewEventEntity>(context);
            var newEvent = new NewEventEntity
            {
                Id = Guid.NewGuid(),
                Name = "Test Event",
                Description = "This is a test event.",
                DateAndTime = DateTime.Now,
                Place = "Test Place",
                Category = "Test Category",
                MaxParticipant = 20,
                Participants = new List<ParticipantEntity>(),
                ImagePath = "Test Image Path"
            };

            // Act
            await repository.AddAsync(newEvent);
            await context.SaveChangesAsync();

            // Assert
            var eventFromDb = await repository.GetByIdAsync(newEvent.Id);
            Assert.NotNull(eventFromDb);
            Assert.Equal("Test Event", eventFromDb.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectEvent()
        {
            // Arrange
            using var context = CreateContext();
            var repository = new Repository<NewEventEntity>(context);
            var newEvent = new NewEventEntity
            {
                Id = Guid.NewGuid(),
                Name = "Test Event 2",
                Description = "This is a test event 2.",
                DateAndTime = DateTime.Now,
                Place = "Test Place 2",
                Category = "Test Category 2",
                MaxParticipant = 50,
                Participants = new List<ParticipantEntity>(),
                ImagePath = "Test Image Path 2"
            };

            await repository.AddAsync(newEvent);
            await context.SaveChangesAsync();

            // Act
            var eventFromDb = await repository.GetByIdAsync(newEvent.Id);

            // Assert
            Assert.NotNull(eventFromDb);
            Assert.Equal("Test Event 2", eventFromDb.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNullForNonExistentId()
        {
            // Arrange
            using var context = CreateContext();
            var repository = new Repository<NewEventEntity>(context);

            // Act
            var eventFromDb = await repository.GetByIdAsync(Guid.NewGuid());

            // Assert
            Assert.Null(eventFromDb);
        }

        [Fact]
        public async Task AddAsync_ShouldNotAddInvalidEvent()
        {
            // Arrange
            using var context = CreateContext();
            var repository = new Repository<NewEventEntity>(context);
            var invalidEvent = new NewEventEntity
            {
                Id = Guid.NewGuid(),
                Name = "", // Неверное имя
                Description = "This is an invalid test event.",
                DateAndTime = DateTime.Now,
                Place = "Test Place",
                Category = "Test Category",
                MaxParticipant = 20,
                Participants = new List<ParticipantEntity>(),
                ImagePath = "Test Path"
            };

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
            {
                if (string.IsNullOrWhiteSpace(invalidEvent.Name))
                    throw new ArgumentException("Name is required.");

                repository.AddAsync(invalidEvent).Wait();
                context.SaveChangesAsync().Wait();
            });
        }


        [Fact]
        public async Task AddAsync_ShouldAddEventWithMaxParticipants()
        {
            // Arrange
            using var context = CreateContext();
            var repository = new Repository<NewEventEntity>(context);
            var maxParticipantsEvent = new NewEventEntity
            {
                Id = Guid.NewGuid(),
                Name = "Max Participants Event",
                Description = "This event has the maximum number of participants.",
                DateAndTime = DateTime.Now,
                Place = "Test Place",
                Category = "Test Category",
                MaxParticipant = 1000, 
                Participants = new List<ParticipantEntity>(),
                ImagePath = "Test Image Path"
            };

            // Act
            await repository.AddAsync(maxParticipantsEvent);
            await context.SaveChangesAsync();

            // Assert
            var eventFromDb = await repository.GetByIdAsync(maxParticipantsEvent.Id);
            Assert.NotNull(eventFromDb);
            Assert.Equal("Max Participants Event", eventFromDb.Name);
            Assert.Equal(1000, eventFromDb.MaxParticipant);
        }
    }
}
