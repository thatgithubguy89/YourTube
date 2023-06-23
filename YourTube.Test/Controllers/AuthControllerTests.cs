using YourTube.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using YourTube.Api.Models;
using YourTube.Api.Models.Requests;
using YourTube.Api.Models.Responses;
using YourTube.Api.Interfaces.Repositories;
using YourTube.Api.Interfaces.Services;

namespace YourTube.Test.Controllers
{
    public class AuthControllerTests
    {
        Mock<IUserRepository> _mockUserRepository;
        Mock<IAuthService> _mockAuthService;
        Mock<ILogger<AuthController>> _mockLogger;

        public AuthControllerTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockAuthService = new Mock<IAuthService>();
            _mockLogger = new Mock<ILogger<AuthController>>();
        }

        [Fact]
        public async Task Signup()
        {
            _mockUserRepository.Setup(u => u.GetUserByUsernameWithoutVideosAsync(It.IsAny<string>())).Returns(Task.FromResult((User?)null));
            _mockUserRepository.Setup(u => u.AddUserAsync(It.IsAny<SignupRequest>()));
            var _authController = new AuthController(_mockLogger.Object, _mockUserRepository.Object, _mockAuthService.Object);

            var actionResult = await _authController.Signup(new SignupRequest());
            var result = actionResult as StatusCodeResult;

            Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
        }

        [Fact]
        public async Task Signup_GivenInvalidSignupRequest_ReturnsBadRequest()
        {
            var _authController = new AuthController(_mockLogger.Object, _mockUserRepository.Object, _mockAuthService.Object);

            var actionResult = await _authController.Signup(null);
            var result = actionResult as BadRequestResult;

            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task Signup_GivenEmailForUserThatExists_ReturnsBadRequest()
        {
            _mockUserRepository.Setup(u => u.GetUserByUsernameWithoutVideosAsync(It.IsAny<string>())).Returns(Task.FromResult(new User()));
            var _authController = new AuthController(_mockLogger.Object, _mockUserRepository.Object, _mockAuthService.Object);

            var actionResult = await _authController.Signup(new SignupRequest());
            var result = actionResult as BadRequestResult;

            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task Signup_Failure_ReturnsStatus500InternalServerError()
        {
            _mockUserRepository.Setup(u => u.GetUserByUsernameWithoutVideosAsync(It.IsAny<string>())).Throws(new Exception());
            var _authController = new AuthController(_mockLogger.Object, _mockUserRepository.Object, _mockAuthService.Object);

            var actionResult = await _authController.Signup(new SignupRequest());
            var result = actionResult as ObjectResult;

            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
        }

        [Fact]
        public async Task Signin()
        {
            _mockAuthService.Setup(a => a.AuthorizeUserAsync(It.IsAny<SigninRequest>())).Returns(Task.FromResult(new SigninResponse()));
            var _authController = new AuthController(_mockLogger.Object, _mockUserRepository.Object, _mockAuthService.Object);

            var actionResult = await _authController.Signin(new SigninRequest());
            var result = actionResult as OkObjectResult;

            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async Task Signin_GivenInvalidSigninRequest_ReturnsBadRequest()
        {
            var _authController = new AuthController(_mockLogger.Object, _mockUserRepository.Object, _mockAuthService.Object);

            var actionResult = await _authController.Signin(null);
            var result = actionResult as BadRequestResult;

            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task Signin_GivenInvalidSigninRequest_ReturnsUnauthorized()
        {
            _mockAuthService.Setup(a => a.AuthorizeUserAsync(It.IsAny<SigninRequest>())).Returns(Task.FromResult((SigninResponse?)null));
            var _authController = new AuthController(_mockLogger.Object, _mockUserRepository.Object, _mockAuthService.Object);

            var actionResult = await _authController.Signin(new SigninRequest());
            var result = actionResult as UnauthorizedResult;

            Assert.Equal(StatusCodes.Status401Unauthorized, result.StatusCode);
        }

        [Fact]
        public async Task Signin_Failure_ReturnsStatus500InternalServerError()
        {
            _mockAuthService.Setup(a => a.AuthorizeUserAsync(It.IsAny<SigninRequest>())).Throws(new Exception());
            var _authController = new AuthController(_mockLogger.Object, _mockUserRepository.Object, _mockAuthService.Object);

            var actionResult = await _authController.Signin(new SigninRequest());
            var result = actionResult as ObjectResult;

            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
        }
    }
}
