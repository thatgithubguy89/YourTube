using Microsoft.EntityFrameworkCore;
using Moq;
using YourTube.Api.Data;
using YourTube.Api.Interfaces.Repositories;
using YourTube.Api.Interfaces.Services;
using YourTube.Api.Models;
using YourTube.Api.Repositories;

namespace YourTube.Test.Repositories
{
    public class LikeRepositoryTests
    {
        ILikeRepository _likeRepository;
        Mock<IVideoRepository> _mockVideoRepository;
        Mock<ICacheService<Video>> _mockCacheService;
        YourTubeContext _context;
        DbContextOptions<YourTubeContext> _options = new DbContextOptionsBuilder<YourTubeContext>()
            .UseInMemoryDatabase("LikeRepositoryTests")
            .Options;

        Like _mockLike = new Like { Id = 1, Liked = true, Username = "test@gmail.com", VideoId = 1 };

        Video _mockVideo = new Video { Id = 1, Title = "test", User = new User { Id = 1 }, Comments = new List<Comment>() };

        public LikeRepositoryTests()
        {
            _context = new YourTubeContext(_options);

            _mockVideoRepository = new Mock<IVideoRepository>();
            _mockVideoRepository.Setup(v => v.GetByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(_mockVideo));
            _mockVideoRepository.Setup(v => v.UpdateAsync(It.IsAny<Video>()));

            _mockCacheService = new Mock<ICacheService<Video>>();
            _mockCacheService.Setup(c => c.DeleteItems(It.IsAny<string>()));

            _likeRepository = new LikeRepository(_context, _mockVideoRepository.Object, _mockCacheService.Object);

            _context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }

        [Fact]
        public async Task AddLikeAsync()
        {
            await _context.Videos.AddAsync(_mockVideo);
            await _context.SaveChangesAsync();

            await _likeRepository.AddAsync(_mockLike);
            var result = await _likeRepository.GetByIdAsync(_mockLike.Id);

            Assert.Equal(_mockLike.Id, result.Id);
            Assert.IsType<Like>(result);
        }

        [Fact]
        public async Task AddLikeAsync_GivenUserHasAlreadyVoted_ReturnsNull()
        {
            await _context.Likes.AddAsync(_mockLike);
            await _context.SaveChangesAsync();

            var result = await _likeRepository.AddAsync(_mockLike);

            Assert.Null(result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task AddLikeAsync_GivenInvalidId_ThrowsArgumentNullException(int id)
        {
            _mockLike.VideoId = id;

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await _likeRepository.AddAsync(_mockLike));
        }

        [Fact]
        public async Task AddLikeAsync_GivenInvalidLike_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _likeRepository.AddAsync(null));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task AddLikeAsync_GivenInvalidUsername_ThrowsArgumentNullException(string username)
        {
            _mockLike.Username = username;

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _likeRepository.AddAsync(_mockLike));
        }
    }
}
