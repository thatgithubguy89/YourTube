using AutoMapper;
using Moq;
using Microsoft.Extensions.Logging;
using YourTube.Api.Controllers;
using YourTube.Api.Profiles;
using YourTube.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using YourTube.Api.Models.Dtos;
using YourTube.Api.Interfaces.Repositories;

namespace YourTube.Test.Controllers
{
    public class TrendingControllerTests
    {
        Mock<ITrendingRepository> _mockTrendingRepository;
        Mock<ILogger<TrendingController>> _mockLogger;
        IMapper _mapper;

        public TrendingControllerTests()
        {
            var config = new MapperConfiguration(c => c.AddProfile(new MappingProfile()));
            _mapper = new Mapper(config);

            _mockLogger = new Mock<ILogger<TrendingController>>();

            _mockTrendingRepository = new Mock<ITrendingRepository>();
        }

        [Fact]
        public async Task GetTrendingVideos()
        {
            _mockTrendingRepository.Setup(t => t.GetTrendingVideos()).Returns(Task.FromResult(new List<Video>()));
            var _trendingController = new TrendingController(_mockTrendingRepository.Object, _mockLogger.Object, _mapper);

            var actionResult = await _trendingController.GetTrendingVideos();
            var result = actionResult as OkObjectResult;

            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.IsType<List<VideoDto>>(result.Value);
        }

        [Fact]
        public async Task GetTrendingVideos_Failure_ReturnsInternalServerError()
        {
            _mockTrendingRepository.Setup(t => t.GetTrendingVideos()).Throws(new Exception());
            var _trendingController = new TrendingController(_mockTrendingRepository.Object, _mockLogger.Object, _mapper);

            var actionResult = await _trendingController.GetTrendingVideos();
            var result = actionResult as ObjectResult;

            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
        }
    }
}
