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
    public class UsersControllerTests
    {
        Mock<IUserRepository> _mockUserRepository;
        IMapper _mapper;
        Mock<ILogger<UsersController>> _mockLogger;

        public UsersControllerTests()
        {
            var config = new MapperConfiguration(c => c.AddProfile(typeof(MappingProfile)));
            _mapper = new Mapper(config);

            _mockLogger = new Mock<ILogger<UsersController>>();

            _mockUserRepository = new Mock<IUserRepository>();
        }

        [Fact]
        public async Task GetSingleUser()
        {
            _mockUserRepository.Setup(u => u.GetUserByUsernameAsync(It.IsAny<string>())).Returns(Task.FromResult(new User()));
            var _usersController = new UsersController(_mockLogger.Object, _mapper, _mockUserRepository.Object);

            var actionResult = await _usersController.GetSingleUser("test@gmail.com");
            var result = actionResult as OkObjectResult;

            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.IsType<UserDto>(result.Value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task GetSingleUser_GivenInvalidUsername_ReturnsBadRequest(string username)
        {
            var _usersController = new UsersController(_mockLogger.Object, _mapper, _mockUserRepository.Object);

            var actionResult = await _usersController.GetSingleUser(username);
            var result = actionResult as BadRequestResult;

            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task GetSingleUser_GivenUsernameForUserThatDoesNotExist_ReturnsNotFound()
        {
            _mockUserRepository.Setup(u => u.GetUserByUsernameAsync(It.IsAny<string>())).Returns(Task.FromResult((User?)null));
            var _usersController = new UsersController(_mockLogger.Object, _mapper, _mockUserRepository.Object);

            var actionResult = await _usersController.GetSingleUser("test@gmail.com");
            var result = actionResult as NotFoundResult;

            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        }

        [Fact]
        public async Task GetSingleUser_Failure_ReturnsStatus500InternalServerError()
        {
            _mockUserRepository.Setup(u => u.GetUserByUsernameAsync(It.IsAny<string>())).Throws(new Exception());
            var _usersController = new UsersController(_mockLogger.Object, _mapper, _mockUserRepository.Object);

            var actionResult = await _usersController.GetSingleUser("test@gmail.com");
            var result = actionResult as ObjectResult;

            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
        }

        [Fact]
        public async Task DeleteUser()
        {
            _mockUserRepository.Setup(u => u.GetByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(new User()));
            _mockUserRepository.Setup(u => u.DeleteAsync(It.IsAny<User>()));
            var _usersController = new UsersController(_mockLogger.Object, _mapper, _mockUserRepository.Object);

            var actionResult = await _usersController.DeleteUser(1);
            var result = actionResult as NoContentResult;

            Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task DeleteUser_GivenInvalidId_ReturnsBadRequest(int id)
        {
            var _usersController = new UsersController(_mockLogger.Object, _mapper, _mockUserRepository.Object);

            var actionResult = await _usersController.DeleteUser(id);
            var result = actionResult as BadRequestResult;

            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task DeleteUser_GivenIdForUserThatDoesNotExist_ReturnsNotFound()
        {
            _mockUserRepository.Setup(u => u.GetByIdAsync(It.IsAny<int>())).Returns(Task.FromResult((User?)null));
            var _usersController = new UsersController(_mockLogger.Object, _mapper, _mockUserRepository.Object);

            var actionResult = await _usersController.DeleteUser(1);
            var result = actionResult as NotFoundResult;

            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        }

        [Fact]
        public async Task DeleteUser_Failure_ReturnsStatus500InternalServerError()
        {
            _mockUserRepository.Setup(u => u.GetByIdAsync(It.IsAny<int>())).Throws(new Exception());
            var _usersController = new UsersController(_mockLogger.Object, _mapper, _mockUserRepository.Object);

            var actionResult = await _usersController.DeleteUser(1);
            var result = actionResult as ObjectResult;

            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
        }
    }
}
