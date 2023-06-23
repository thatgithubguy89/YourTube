using Microsoft.EntityFrameworkCore;
using Moq;
using YourTube.Api.Data;
using YourTube.Api.Interfaces.Repositories;
using YourTube.Api.Models;
using YourTube.Api.Repositories;

namespace YourTube.Test.Repositories
{
    public class VideoViewRepositoryTests : IDisposable
    {
        IVideoViewRepository _videoViewRepository;
        Mock<IVideoRepository> _mockVideoRepository;
        YourTubeContext _context;
        DbContextOptions<YourTubeContext> _options = new DbContextOptionsBuilder<YourTubeContext>()
            .UseInMemoryDatabase("VideoViewRepositoryTests")
            .Options;

        public VideoViewRepositoryTests()
        {
            _context = new YourTubeContext(_options);

            _mockVideoRepository = new Mock<IVideoRepository>();
            _mockVideoRepository.Setup(v => v.GetByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(new Video()));
            _mockVideoRepository.Setup(v => v.UpdateAsync(It.IsAny<Video>()));

            _videoViewRepository = new VideoViewRepository(_context, _mockVideoRepository.Object);

            _context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }

        [Fact]
        public async Task AddVideoViewAsync()
        {
            var videoView = new VideoView { Id = 1, VideoId = 1, Username = "test@gmail.com" };

            await _videoViewRepository.AddVideoViewAsync(videoView);
            var result = await _videoViewRepository.GetByIdAsync(1);

            Assert.Equal(1, result.VideoId);
            Assert.Equal("test@gmail.com", result.Username);
            Assert.IsType<VideoView>(result);
            _mockVideoRepository.Verify(m => m.GetByIdAsync(1), Times.Once());
            _mockVideoRepository.Verify(m => m.UpdateAsync(It.IsAny<Video>()), Times.Once());
        }

        [Fact]
        public async Task AddVideoViewAsync_GivenVideoViewThatAlreadyExists_NothingHappens()
        {
            var videoView = new VideoView { Id = 1, VideoId = 1, Username = "test@gmail.com" };
            await _videoViewRepository.AddAsync(videoView);

            await _videoViewRepository.AddVideoViewAsync(videoView);

            _mockVideoRepository.Verify(m => m.GetByIdAsync(1), Times.Never());
            _mockVideoRepository.Verify(m => m.UpdateAsync(It.IsAny<Video>()), Times.Never());
        }

        [Fact]
        public async Task AddVideoViewAsync_GivenVideoIdForVideoThatDoesNotExists_ThrowsNullReferenceException()
        {
            _mockVideoRepository.Setup(v => v.GetByIdAsync(It.IsAny<int>())).Returns(Task.FromResult((Video)null));
            var videoView = new VideoView { Id = 1, VideoId = 1, Username = "test@gmail.com" };

            await Assert.ThrowsAsync<NullReferenceException>(async () => await _videoViewRepository.AddVideoViewAsync(videoView));
        }
    }
}
