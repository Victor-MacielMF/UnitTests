using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Models;
using RunGroopWebApp.Repository;
using RunGroopWebApp.Data.Enum;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using System.Linq;

namespace RunGroopWebApp.Tests.Repository
{
    public class ClubRepositoryTests
    {
        private ApplicationDbContext GetDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var context = new ApplicationDbContext(options);

            context.Clubs.AddRange
                (
                    new Club
                    {
                        Id = 1,
                        Title = "Test Club 1",
                        ClubCategory = ClubCategory.City,
                        Address = new Address
                        {
                            Street = "123 Main St",
                            City = "Springfield",
                            State = "IL",
                            ZipCode = 62701
                        }
                    },
                    new Club
                    {
                        Id = 2,
                        Title = "Test Club 2",
                        ClubCategory = ClubCategory.Trail,
                        Address = new Address
                        {
                            Street = "456 Elm St",
                            City = "Metropolis",
                            State = "NY",
                            ZipCode = 10001
                        }
                    }
                );

            context.States.Add(new State { Id = 1, StateName = "Illinois" , StateCode = "IL"});
            context.Cities.Add(new City
            {
                Id = 1,
                CityName = "Springfield",
                StateCode = "IL",
                Zip = 62701,
                Latitude = 39.798976,
                Longitude = -89.644368,
                County = "Sangamon"
            });

            context.SaveChanges();
            return context;
        }

        [Fact]
        public async Task GetAll_ReturnsAllClubs()
        {
            var context = GetDbContext("GetAllDb");
            var repo = new ClubRepository(context);

            var result = await repo.GetAll();

            result.Count().Should().Be(2);
        }

        [Fact]
        public async Task GetAllStates_ReturnsStates()
        {
            var context = GetDbContext("GetStatesDb");
            var repo = new ClubRepository(context);

            var result = await repo.GetAllStates();

            result.Should().ContainSingle(s => s.StateName == "Illinois");
        }

        [Fact]
        public async Task GetSliceAsync_ReturnsCorrectSlice()
        {
            var context = GetDbContext("GetSliceDb");
            var repo = new ClubRepository(context);

            var result = await repo.GetSliceAsync(0, 1);

            result.Count().Should().Be(1);
        }

        [Fact]
        public async Task GetClubsByCategoryAndSliceAsync_ReturnsFilteredAndSliced()
        {
            var context = GetDbContext("CategorySliceDb");
            var repo = new ClubRepository(context);

            var result = await repo.GetClubsByCategoryAndSliceAsync(ClubCategory.City, 0, 10);

            result.Should().ContainSingle();
        }

        [Fact]
        public async Task GetCountByCategoryAsync_ReturnsCorrectCount()
        {
            var context = GetDbContext("CountByCategoryDb");
            var repo = new ClubRepository(context);

            var result = await repo.GetCountByCategoryAsync(ClubCategory.City);

            result.Should().Be(1);
        }

        [Fact]
        public async Task GetByIdAsyncNoTracking_ReturnsClub()
        {
            var context = GetDbContext("GetByIdNoTrackingDb");
            var repo = new ClubRepository(context);

            var result = await repo.GetByIdAsyncNoTracking(1);

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetClubByCity_ReturnsCorrectClubs()
        {
            var context = GetDbContext("GetClubByCityDb");
            var repo = new ClubRepository(context);

            var result = await repo.GetClubByCity("Springfield");

            result.Should().ContainSingle();
        }

        [Fact]
        public async Task GetCountAsync_ReturnsTotalCount()
        {
            var context = GetDbContext("GetCountDb");
            var repo = new ClubRepository(context);

            var result = await repo.GetCountAsync();

            result.Should().Be(2);
        }

        [Fact]
        public async Task GetClubsByState_ReturnsCorrectClubs()
        {
            var context = GetDbContext("GetClubsByStateDb");
            var repo = new ClubRepository(context);

            var result = await repo.GetClubsByState("IL");

            result.Should().ContainSingle();
        }

        [Fact]
        public async Task GetAllCitiesByState_ReturnsCities()
        {
            var context = GetDbContext("GetCitiesByStateDb");
            var repo = new ClubRepository(context);

            var result = await repo.GetAllCitiesByState("IL");

            result.Should().ContainSingle(c => c.CityName == "Springfield");
        }

        [Fact]
        public void Add_AddsClub()
        {
            var context = GetDbContext("AddClubDb");
            var repo = new ClubRepository(context);
            var newClub = new Club
            {
                Id = 3,
                Title = "New Club",
                Address = new Address
                {
                    Id = 3,
                    Street = "789 Oak Avenue",
                    City = "Gotham",
                    State = "NJ",
                    ZipCode = 07001
                }
            };


            var result = repo.Add(newClub);

            result.Should().BeTrue();
            context.Clubs.Should().Contain(c => c.Title == "New Club");
        }

        [Fact]
        public void Update_UpdatesClub()
        {
            var context = GetDbContext("UpdateClubDb");
            var repo = new ClubRepository(context);

            var club = context.Clubs.First();
            club.Title = "Updated Title";

            var result = repo.Update(club);

            result.Should().BeTrue();
            context.Clubs.First().Title.Should().Be("Updated Title");
        }

        [Fact]
        public void Delete_RemovesClub()
        {
            var context = GetDbContext("DeleteClubDb");
            var repo = new ClubRepository(context);

            var club = context.Clubs.First();

            var result = repo.Delete(club);

            result.Should().BeTrue();
            context.Clubs.Should().NotContain(club);
        }
    }
}
