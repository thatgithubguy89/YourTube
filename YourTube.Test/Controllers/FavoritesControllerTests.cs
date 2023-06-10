using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YourTube.Api.Controllers;
using YourTube.Api.Models.Dtos;
using YourTube.Api.Models;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using YourTube.Api.Profiles;
using YourTube.Api.Interfaces;

namespace YourTube.Test.Controllers
{
    public class FavoritesControllerTests
    {
        IMapper _mapper;
        Mock<ILogger<FavoritesController>> _mockLogger;
        Mock<IVideoRepository> _mockVideoRepository;
        Mock<IFavoriteRepository> _mockFavoriteRepository;

        public FavoritesControllerTests()
        {
            var config = new MapperConfiguration(c => c.AddProfile(typeof(MappingProfile)));
            _mapper = new Mapper(config);

            _mockLogger = new Mock<ILogger<FavoritesController>>();

            _mockVideoRepository = new Mock<IVideoRepository>();

            _mockFavoriteRepository = new Mock<IFavoriteRepository>();
        }

        [Fact]
        public async Task GetUsersFavoriteVideos()
        {
            _mockVideoRepository.Setup(v => v.GetFavoriteVideosByUsernameAsync(It.IsAny<string>())).Returns(Task.FromResult(new List<Video>()));
            var _favoritesController = new FavoritesController(_mockFavoriteRepository.Object, _mockVideoRepository.Object, _mapper, _mockLogger.Object);

            var actionResult = await _favoritesController.GetUsersFavoriteVideos("test@gmail.com");
            var result = actionResult as OkObjectResult;

            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.IsType<List<VideoDto>>(result.Value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task GetUsersFavoriteVideos_GivenInvalidUsername_ReturnsBadRequest(string username)
        {
            var _favoritesController = new FavoritesController(_mockFavoriteRepository.Object, _mockVideoRepository.Object, _mapper, _mockLogger.Object);

            var actionResult = await _favoritesController.GetUsersFavoriteVideos(username);
            var result = actionResult as BadRequestResult;

            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task GetUsersFavoriteVideos_Failure_ReturnsInternalServerError()
        {
            _mockVideoRepository.Setup(v => v.GetFavoriteVideosByUsernameAsync(It.IsAny<string>())).Throws(new Exception());
            var _favoritesController = new FavoritesController(_mockFavoriteRepository.Object, _mockVideoRepository.Object, _mapper, _mockLogger.Object);

            var actionResult = await _favoritesController.GetUsersFavoriteVideos("test@gmail.com");
            var result = actionResult as ObjectResult;

            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
        }

        [Fact]
        public async Task CreateFavorite()
        {
            _mockFavoriteRepository.Setup(f => f.AddAsync(It.IsAny<Favorite>()));
            var _favoritesController = new FavoritesController(_mockFavoriteRepository.Object, _mockVideoRepository.Object, _mapper, _mockLogger.Object);

            var actionResult = await _favoritesController.CreateFavorite(new FavoriteDto());
            var result = actionResult as StatusCodeResult;

            Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
        }

        [Fact]
        public async Task CreateFavorite_GivenInvalidFavorite_ReturnsBadRequest()
        {
            var _favoritesController = new FavoritesController(_mockFavoriteRepository.Object, _mockVideoRepository.Object, _mapper, _mockLogger.Object);

            var actionResult = await _favoritesController.CreateFavorite(null);
            var result = actionResult as BadRequestResult;

            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task CreateFavorite_Failure_ReturnsInternalServerError()
        {
            _mockFavoriteRepository.Setup(f => f.AddAsync(It.IsAny<Favorite>())).Throws(new Exception());
            var _favoritesController = new FavoritesController(_mockFavoriteRepository.Object, _mockVideoRepository.Object, _mapper, _mockLogger.Object);

            var actionResult = await _favoritesController.CreateFavorite(new FavoriteDto());
            var result = actionResult as ObjectResult;

            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
        }

        [Fact]
        public async Task DeleteFavorite()
        {
            _mockFavoriteRepository.Setup(f => f.DeleteAsync(It.IsAny<Favorite>()));
            var _favoritesController = new FavoritesController(_mockFavoriteRepository.Object, _mockVideoRepository.Object, _mapper, _mockLogger.Object);

            var actionResult = await _favoritesController.DeleteFavorite(new FavoriteDto());
            var result = actionResult as NoContentResult;

            Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
        }

        [Fact]
        public async Task DeleteFavorite_GivenInvalidFavorite_ReturnsBadRequest()
        {
            var _favoritesController = new FavoritesController(_mockFavoriteRepository.Object, _mockVideoRepository.Object, _mapper, _mockLogger.Object);

            var actionResult = await _favoritesController.DeleteFavorite(null);
            var result = actionResult as BadRequestResult;

            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task DeleteFavorite_Failure_ReturnsInternalServerError()
        {
            _mockFavoriteRepository.Setup(f => f.DeleteAsync(It.IsAny<Favorite>())).Throws(new Exception());
            var _favoritesController = new FavoritesController(_mockFavoriteRepository.Object, _mockVideoRepository.Object, _mapper, _mockLogger.Object);

            var actionResult = await _favoritesController.DeleteFavorite(new FavoriteDto());
            var result = actionResult as ObjectResult;

            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
        }
    }
}
