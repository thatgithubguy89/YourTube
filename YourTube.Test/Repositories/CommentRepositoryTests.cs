using Microsoft.EntityFrameworkCore;
using Moq;
using YourTube.Api.Data;
using YourTube.Api.Interfaces.Repositories;
using YourTube.Api.Interfaces.Services;
using YourTube.Api.Models;
using YourTube.Api.Repositories;

namespace YourTube.Test.Repositories
{
    public class CommentRepositoryTests : IDisposable
    {
        ICommentRepository _commentRepository;
        Mock<ICacheService<Video>> _mockCacheService;
        YourTubeContext _context;
        DbContextOptions<YourTubeContext> _options = new DbContextOptionsBuilder<YourTubeContext>()
            .UseInMemoryDatabase("CommentRepositoryTests")
            .Options;

        Comment _mockComment = new Comment { Id = 1, Content = "test" };

        public CommentRepositoryTests()
        {
            _context = new YourTubeContext(_options);

            _mockCacheService = new Mock<ICacheService<Video>>();
            _mockCacheService.Setup(c => c.DeleteItems(It.IsAny<string>()));

            _commentRepository = new CommentRepository(_context, _mockCacheService.Object);

            _context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }

        [Fact]
        public async Task AddAsync()
        {
            var result = await _commentRepository.AddAsync(_mockComment);

            Assert.Equal(_mockComment.Id, result.Id);
            Assert.Equal(_mockComment.Content, result.Content);
            Assert.IsType<Comment>(result);
        }

        [Fact]
        public async Task AddAsync_GivenInvalidComment_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _commentRepository.AddAsync(null));
        }

        [Fact]
        public async Task DeleteAsync()
        {
            await _commentRepository.AddAsync(_mockComment);

            await _commentRepository.DeleteAsync(_mockComment);
            var result = await _commentRepository.GetByIdAsync(1);

            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteAsync_GivenInvalidComment_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _commentRepository.DeleteAsync(null));
        }
    }
}
