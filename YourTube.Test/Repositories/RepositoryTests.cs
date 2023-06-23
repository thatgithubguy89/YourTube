using Microsoft.EntityFrameworkCore;
using YourTube.Api.Data;
using YourTube.Api.Interfaces.Repositories;
using YourTube.Api.Models;
using YourTube.Api.Repositories;

namespace YourTube.Test.Repositories
{
    [Collection("Sequential")]
    public class RepositoryTests : IDisposable
    {
        IRepository<Video> _repository;
        YourTubeContext _context;
        DbContextOptions<YourTubeContext> _options = new DbContextOptionsBuilder<YourTubeContext>()
            .UseInMemoryDatabase("RepositoryTests")
            .Options;

        Video _mockVideo = new Video { Id = 1, Title = "test" };

        List<Video> _mockVideos = new List<Video>
        {
            new Video { Id = 1, Title = "test" },
            new Video { Id = 2, Title = "test" },
            new Video { Id = 3, Title = "test" }
        };

        public RepositoryTests()
        {
            _context = new YourTubeContext(_options);

            _repository = new Repository<Video>(_context);

            _context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }

        [Fact]
        public async Task AddAsync()
        {
            var resut = await _repository.AddAsync(_mockVideo);

            Assert.Equal(1, resut.Id);
            Assert.IsType<Video>(resut);
        }

        [Fact]
        public async Task AddAsync_GivenInvalidVideo_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _repository.AddAsync(null));
        }

        [Fact]
        public async Task DeleteAsync()
        {
            await _repository.AddAsync(_mockVideo);

            await _repository.DeleteAsync(_mockVideo);
            var result = await _repository.GetByIdAsync(_mockVideo.Id);

            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteAsync_GivenInvalidVideo_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _repository.DeleteAsync(null));
        }

        [Fact]
        public async Task GetAllAsync()
        {
            await _context.AddRangeAsync(_mockVideos);
            await _context.SaveChangesAsync();

            var result = await _repository.GetAllAsync();

            Assert.Equal(3, result.Count);
            Assert.IsType<List<Video>>(result);
        }

        [Fact]
        public async Task GetByIdAsync()
        {
            await _repository.AddAsync(_mockVideo);

            var result = await _repository.GetByIdAsync(_mockVideo.Id);

            Assert.Equal(1, result.Id);
            Assert.IsType<Video>(result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetByIdAsync_GivenInvalidId_ThrowsArgumentOutOfRangeException(int id)
        {
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await _repository.GetByIdAsync(id));
        }

        [Fact]
        public async Task UpdateAsync()
        {
            await _repository.AddAsync(_mockVideo);
            _mockVideo.Title = "test2";

            await _repository.UpdateAsync(_mockVideo);
            var result = await _repository.GetByIdAsync(1);

            Assert.Equal(1, result.Id);
            Assert.Equal("test2", result.Title);
            Assert.IsType<Video>(result);
        }

        [Fact]
        public async Task UpdateAsync_GivenInvalidVideo_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _repository.UpdateAsync(null));
        }
    }
}
