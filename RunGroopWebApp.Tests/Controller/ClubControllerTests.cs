using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp.Controllers;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;
using RunGroopWebApp.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace RunGroopWebApp.Tests.Controller
{
    public class ClubControllerTests
    {
        private readonly IClubRepository _clubRepository = A.Fake<IClubRepository>();
        private readonly IPhotoService _photoService = A.Fake<IPhotoService>();
        private readonly ClubController _controller;

        public ClubControllerTests()
        {
            _controller = new ClubController(_clubRepository, _photoService);
        }

        [Fact]
        public async Task Index_ReturnsNotFound_WhenPageOrPageSizeIsInvalid()
        {
            // Act
            var result = await _controller.Index(page: 0, pageSize: 0);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithCorrectViewModel_WhenParametersAreValid()
        {
            // Arrange
            var fakeClubs = new List<Club> { A.Fake<Club>(), A.Fake<Club>() };
            A.CallTo(() => _clubRepository.GetSliceAsync(0, 6)).Returns(fakeClubs);
            A.CallTo(() => _clubRepository.GetCountAsync()).Returns(2);

            // Act
            var result = await _controller.Index();

            // Assert
            result.Should().BeOfType<ViewResult>();

            var viewResult = result as ViewResult;
            viewResult!.Model.Should().BeOfType<IndexClubViewModel>();

            var model = viewResult.Model as IndexClubViewModel;
            model!.Clubs.Should().HaveCount(2);
            model.TotalClubs.Should().Be(2);
            model.TotalPages.Should().Be(1);
            model.Page.Should().Be(1);
            model.PageSize.Should().Be(6);
        }
    }
}
