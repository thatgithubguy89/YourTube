using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using YourTube.Api.Controllers;
using YourTube.Api.Interfaces;
using YourTube.Api.Models;
using YourTube.Api.Models.Dtos;
using YourTube.Api.Profiles;

namespace YourTube.Test.Controllers
{
    public class LikesControllerTests
    {
        Mock<ILikeRepository> _mockLikeRepository;
        IMapper _mapper;
        Mock<ILogger<LikesController>> _mockLogger;

        public LikesControllerTests()
        {
            var config = new MapperConfiguration(c => c.AddProfile(typeof(MappingProfile)));
            _mapper = new Mapper(config);

            _mockLogger = new Mock<ILogger<LikesController>>();

            _mockLikeRepository = new Mock<ILikeRepository>();
        }

        [Fact]
        public async Task CreateLike()
        {
            _mockLikeRepository.Setup(l => l.AddAsync(It.IsAny<Like>())).Returns(Task.FromResult(new Like()));
            var _likesController = new LikesController(_mockLogger.Object, _mockLikeRepository.Object, _mapper);

            var actionResult = await _likesController.CreateLike(new LikeDto());
            var result = actionResult as StatusCodeResult;

            Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
        }

        [Fact]
        public async Task CreateLike_GivenUserHasLikedVideo_ReturnsNoContent()
        {
            _mockLikeRepository.Setup(l => l.AddAsync(It.IsAny<Like>()));
            var _likesController = new LikesController(_mockLogger.Object, _mockLikeRepository.Object, _mapper);

            var actionResult = await _likesController.CreateLike(new LikeDto());
            var result = actionResult as NoContentResult;

            Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
        }

        [Fact]
        public async Task CreateLike_GivenInvalidLike_ReturnsBadRequest()
        {
            var _likesController = new LikesController(_mockLogger.Object, _mockLikeRepository.Object, _mapper);

            var actionResult = await _likesController.CreateLike(null);
            var result = actionResult as BadRequestResult;

            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task CreateLike_Failure_ReturnsInternalServerError()
        {
            _mockLikeRepository.Setup(l => l.AddAsync(It.IsAny<Like>())).Throws(new Exception());
            var _likesController = new LikesController(_mockLogger.Object, _mockLikeRepository.Object, _mapper);

            var actionResult = await _likesController.CreateLike(new LikeDto());
            var result = actionResult as ObjectResult;

            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
        }
    }
}
