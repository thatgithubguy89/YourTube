using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using YourTube.Api.Controllers;
using YourTube.Api.Interfaces.Services;
using YourTube.Api.Models;
using YourTube.Api.Models.Dtos;
using YourTube.Api.Profiles;

namespace YourTube.Test.Controllers
{
    public class RecommendControllerTests
    {
        Mock<IRecommendService> _mockRecommendService;
        Mock<ILogger<RecommendController>> _mockLogger;
        IMapper _mapper;

        public RecommendControllerTests()
        {
            var config = new MapperConfiguration(c => c.AddProfile(typeof(MappingProfile)));
            _mapper = new Mapper(config);

            _mockLogger = new Mock<ILogger<RecommendController>>();

            _mockRecommendService = new Mock<IRecommendService>();
            _mockRecommendService.Setup(r => r.GetRecommendedVideosAsync(It.IsAny<int>(), It.IsAny<List<Tag>>())).Returns(Task.FromResult(new List<Video>()));
        }

        [Fact]
        public async Task GetRecommendedVideos()
        {
            var _recommendController = new RecommendController(_mockRecommendService.Object, _mockLogger.Object, _mapper);

            var actionResult = await _recommendController.GetRecommendedVideos(1, new List<TagDto>() { new TagDto() });
            var result = actionResult as OkObjectResult;

            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.IsType<List<VideoDto>>(result.Value);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetRecommendedVideos_GivenInvalidId_ReturnsBadRequest(int id)
        {
            var _recommendController = new RecommendController(_mockRecommendService.Object, _mockLogger.Object, _mapper);

            var actionResult = await _recommendController.GetRecommendedVideos(id, new List<TagDto>());
            var result = actionResult as BadRequestResult;

            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task GetRecommendedVideos_GivenEmptyTags_ReturnsBadRequest()
        {
            var _recommendController = new RecommendController(_mockRecommendService.Object, _mockLogger.Object, _mapper);

            var actionResult = await _recommendController.GetRecommendedVideos(1, new List<TagDto>());
            var result = actionResult as BadRequestResult;

            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task GetRecommendedVideos_Failure_ReturnsInternalServerError()
        {
            var _recommendController = new RecommendController(_mockRecommendService.Object, _mockLogger.Object, _mapper);
            _mockRecommendService.Setup(r => r.GetRecommendedVideosAsync(It.IsAny<int>(), It.IsAny<List<Tag>>())).Throws(new Exception());

            var actionResult = await _recommendController.GetRecommendedVideos(1, new List<TagDto>() { new TagDto() });
            var result = actionResult as ObjectResult;

            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
        }
    }
}
