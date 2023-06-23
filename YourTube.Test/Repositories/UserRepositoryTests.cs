using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourTube.Api.Data;
using YourTube.Api.Interfaces.Repositories;
using YourTube.Api.Interfaces.Services;
using YourTube.Api.Models;
using YourTube.Api.Models.Requests;
using YourTube.Api.Repositories;

namespace YourTube.Test.Repositories
{
    public class UserRepositoryTests : IDisposable
    {
        IUserRepository _userRepository;
        Mock<IFileService> _mockFileService;
        YourTubeContext _context;
        DbContextOptions<YourTubeContext> _options = new DbContextOptionsBuilder<YourTubeContext>()
            .UseInMemoryDatabase("UserRepositoryTests")
            .Options;

        User _mockUser = new User { Id = 1, Username = "test@gmail.com" };

        IFormFile _mockFile = new FormFile(new MemoryStream(new byte[1]), 0, 1, "test", "test");

        public UserRepositoryTests()
        {
            _context = new YourTubeContext(_options);

            _mockFileService = new Mock<IFileService>();
            _mockFileService.Setup(f => f.UploadFileAsync(It.IsAny<string>(), It.IsAny<IFormFile>())).Returns(Task.FromResult("test"));
            _mockFileService.Setup(f => f.DeleteFile(It.IsAny<string>()));

            _userRepository = new UserRepository(_context, _mockFileService.Object);

            _context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }

        [Fact]
        public async Task AddUserAsync()
        {
            var signUpRequest = new SignupRequest { Email = "test@gamil.com", Password = "test", File = _mockFile };
            await _userRepository.AddUserAsync(signUpRequest);

            var result = await _userRepository.GetByIdAsync(1);

            Assert.Equal(1, result.Id);
            Assert.IsType<User>(result);
        }

        [Fact]
        public async Task AddUserAsync_GivenInvalidUser_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _userRepository.AddUserAsync(null));
        }

        [Fact]
        public async Task DeleteUserAsync()
        {
            await _userRepository.AddAsync(_mockUser);

            await _userRepository.DeleteAsync(_mockUser);
            var result = await _userRepository.GetByIdAsync(_mockUser.Id);

            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteUserAsync_GivenInvalidUser_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _userRepository.DeleteAsync(null));
        }

        [Fact]
        public async Task GetUserByUsernameAsync()
        {
            await _userRepository.AddAsync(_mockUser);

            var result = await _userRepository.GetUserByUsernameAsync(_mockUser.Username);

            Assert.Equal(_mockUser.Username, result.Username);
            Assert.IsType<User>(result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task GetUserByUsernameAsync_GivenInvalidUsername_ThrowsArgumentNullException(string username)
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _userRepository.GetUserByUsernameAsync(username));
        }

        [Fact]
        public async Task GetUserByUsernameWithoutVideosAsync()
        {
            await _userRepository.AddAsync(_mockUser);

            var result = await _userRepository.GetUserByUsernameWithoutVideosAsync(_mockUser.Username);

            Assert.Equal(_mockUser.Username, result.Username);
            Assert.IsType<User>(result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task GetUserByUsernameWithoutVideosAsync_GivenInvalidUsername_ThrowsArgumentNullException(string username)
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _userRepository.GetUserByUsernameWithoutVideosAsync(username));
        }
    }
}
