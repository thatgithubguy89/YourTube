using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using YourTube.Api.Controllers;
using YourTube.Api.Interfaces;
using YourTube.Api.Models;
using YourTube.Api.Models.Dtos;
using YourTube.Api.Models.Requests;
using YourTube.Api.Profiles;

namespace YourTube.Test.Controllers
{
    public class VideosControllerTests
    {
        Mock<IVideoRepository> _mockVideoRepository;
        IMapper _mapper;
        Mock<ILogger<VideosController>> _mockLogger;

        public VideosControllerTests()
        {
            var config = new MapperConfiguration(c => c.AddProfile(typeof(MappingProfile)));
            _mapper = new Mapper(config);

            _mockLogger = new Mock<ILogger<VideosController>>();

            _mockVideoRepository = new Mock<IVideoRepository>();
        }

        [Fact]
        public async Task GetAll()
        {
            _mockVideoRepository.Setup(v => v.GetAllAsync()).Returns(Task.FromResult(new List<Video>()));
            var _videosController = new VideosController(_mockLogger.Object, _mapper, _mockVideoRepository.Object);

            var actionResult = await _videosController.GetAll();
            var result = actionResult as OkObjectResult;

            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.IsType<List<VideoDto>>(result.Value);
        }

        [Fact]
        public async Task GetAll_Failure_ReturnsInternalServerError()
        {
            _mockVideoRepository.Setup(v => v.GetAllAsync()).Throws(new Exception());
            var _videosController = new VideosController(_mockLogger.Object, _mapper, _mockVideoRepository.Object);

            var actionResult = await _videosController.GetAll();
            var result = actionResult as ObjectResult;

            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
        }

        [Fact]
        public async Task GetSingleVideo()
        {
            _mockVideoRepository.Setup(v => v.GetByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(new Video()));
            var _videosController = new VideosController(_mockLogger.Object, _mapper, _mockVideoRepository.Object);

            var actionResult = await _videosController.GetSingleVideo(1);
            var result = actionResult as OkObjectResult;

            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.IsType<VideoDto>(result.Value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetSingleVideo_GivenInvalidId_ReturnsBadRequest(int id)
        {
            var _videosController = new VideosController(_mockLogger.Object, _mapper, _mockVideoRepository.Object);

            var actionResult = await _videosController.GetSingleVideo(id);
            var result = actionResult as BadRequestResult;

            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task GetSingleVideo_GivenIdForUserThatDoesNotExist_ReturnsNotFound()
        {
            _mockVideoRepository.Setup(v => v.GetByIdAsync(It.IsAny<int>())).Returns(Task.FromResult((Video?)null));
            var _videosController = new VideosController(_mockLogger.Object, _mapper, _mockVideoRepository.Object);

            var actionResult = await _videosController.GetSingleVideo(1);
            var result = actionResult as NotFoundResult;

            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        }

        [Fact]
        public async Task GetSingleVideo_Failure_ReturnsInternalServerError()
        {
            _mockVideoRepository.Setup(v => v.GetByIdAsync(It.IsAny<int>())).Throws(new Exception());
            var _videosController = new VideosController(_mockLogger.Object, _mapper, _mockVideoRepository.Object);

            var actionResult = await _videosController.GetSingleVideo(1);
            var result = actionResult as ObjectResult;

            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
        }

        [Fact]
        public async Task CreateVideo()
        {
            _mockVideoRepository.Setup(v => v.AddVideoAsync(It.IsAny<AddVideoRequest>()));
            var _videosController = new VideosController(_mockLogger.Object, _mapper, _mockVideoRepository.Object);

            var actionResult = await _videosController.CreateVideo(new AddVideoRequest());
            var result = actionResult as NoContentResult;

            Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
        }

        [Fact]
        public async Task CreateVideo_GivenInvalidAddVideoRequest_ReturnsBadRequest()
        {
            var _videosController = new VideosController(_mockLogger.Object, _mapper, _mockVideoRepository.Object);

            var actionResult = await _videosController.CreateVideo(null);
            var result = actionResult as BadRequestResult;

            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task CreateVideo_Failure_ReturnsInternalServerError()
        {
            _mockVideoRepository.Setup(v => v.AddVideoAsync(It.IsAny<AddVideoRequest>())).Throws(new Exception());
            var _videosController = new VideosController(_mockLogger.Object, _mapper, _mockVideoRepository.Object);

            var actionResult = await _videosController.CreateVideo(new AddVideoRequest());
            var result = actionResult as ObjectResult;

            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
        }

        [Fact]
        public async Task DeleteVideo()
        {
            _mockVideoRepository.Setup(v => v.DeleteAsync(It.IsAny<Video>()));
            _mockVideoRepository.Setup(v => v.GetByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(new Video()));
            var _videosController = new VideosController(_mockLogger.Object, _mapper, _mockVideoRepository.Object);

            var actionResult = await _videosController.DeleteVideo(1);
            var result = actionResult as NoContentResult;

            Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task DeleteVideo_GivenInvalidId_ReturnsBadRequest(int id)
        {
            var _videosController = new VideosController(_mockLogger.Object, _mapper, _mockVideoRepository.Object);

            var actionResult = await _videosController.DeleteVideo(id);
            var result = actionResult as BadRequestResult;

            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task DeleteVideo_GivenIdForUserThatDoesNotExists_ReturnsNotFound()
        {
            _mockVideoRepository.Setup(v => v.GetByIdAsync(It.IsAny<int>())).Returns(Task.FromResult((Video?)null));
            var _videosController = new VideosController(_mockLogger.Object, _mapper, _mockVideoRepository.Object);

            var actionResult = await _videosController.DeleteVideo(1);
            var result = actionResult as NotFoundResult;

            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        }

        [Fact]
        public async Task DeleteVideo_Failure_ReturnsInternalServerError()
        {
            _mockVideoRepository.Setup(v => v.GetByIdAsync(It.IsAny<int>())).Throws(new Exception());
            var _videosController = new VideosController(_mockLogger.Object, _mapper, _mockVideoRepository.Object);

            var actionResult = await _videosController.DeleteVideo(1);
            var result = actionResult as ObjectResult;

            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
        }
    }
}
