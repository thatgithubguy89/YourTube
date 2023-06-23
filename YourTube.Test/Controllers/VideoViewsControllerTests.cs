using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using YourTube.Api.Controllers;
using YourTube.Api.Interfaces.Repositories;
using YourTube.Api.Models;
using YourTube.Api.Models.Dtos;
using YourTube.Api.Profiles;

namespace YourTube.Test.Controllers
{
    public class VideoViewsControllerTests
    {
        Mock<IVideoViewRepository> _mockVideoViewRepository;
        IMapper _mapper;
        Mock<ILogger<VideoViewsController>> _mockLogger;

        public VideoViewsControllerTests()
        {
            var config = new MapperConfiguration(c => c.AddProfile(typeof(MappingProfile)));
            _mapper = new Mapper(config);

            _mockLogger = new Mock<ILogger<VideoViewsController>>();

            _mockVideoViewRepository = new Mock<IVideoViewRepository>();
        }

        [Fact]
        public async Task CreateVideoView()
        {
            _mockVideoViewRepository.Setup(v => v.AddVideoViewAsync(It.IsAny<VideoView>()));
            var _videoViewsController = new VideoViewsController(_mockLogger.Object, _mockVideoViewRepository.Object, _mapper);

            var actionResult = await _videoViewsController.CreateVideoView(new VideoViewDto());
            var result = actionResult as NoContentResult;

            Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
        }

        [Fact]
        public async Task CreateVideoView_GivenInvalidVideoView_ReturnsBadRequest()
        {
            var _videoViewsController = new VideoViewsController(_mockLogger.Object, _mockVideoViewRepository.Object, _mapper);

            var actionResult = await _videoViewsController.CreateVideoView(null);
            var result = actionResult as BadRequestResult;

            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task CreateVideoView_Failure_ReturnsInternalServerError()
        {
            _mockVideoViewRepository.Setup(v => v.AddVideoViewAsync(It.IsAny<VideoView>())).Throws(new Exception());
            var _videoViewsController = new VideoViewsController(_mockLogger.Object, _mockVideoViewRepository.Object, _mapper);

            var actionResult = await _videoViewsController.CreateVideoView(new VideoViewDto());
            var result = actionResult as ObjectResult;

            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
        }
    }
}
