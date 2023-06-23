using Microsoft.EntityFrameworkCore;
using YourTube.Api.Data;
using YourTube.Api.Interfaces;
using YourTube.Api.Models;
using YourTube.Api.Repositories;

namespace YourTube.Test.Repositories
{
    public class TagRepositoryTests : IDisposable
    {
        ITagRepository _tagRepository;
        DbContextOptions<YourTubeContext> _options = new DbContextOptionsBuilder<YourTubeContext>()
            .UseInMemoryDatabase("TagRepositoryTests")
            .Options;
        YourTubeContext _context;

        Video _mockVideo = new Video { Id = 1, Title = "test", User = new User { Id = 1 }, Comments = new List<Comment>() };

        List<Tag> _mockTags = new List<Tag>
        {
            new Tag {Id = 1, Name = "tag1"},
            new Tag {Id = 2, Name = "tag2"},
            new Tag {Id = 3, Name = "tag3"}
        };

        public TagRepositoryTests()
        {
            _context = new YourTubeContext(_options);

            _tagRepository = new TagRepository(_context);

            _context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }

        [Fact]
        public async Task AddTagsForVideoAsync()
        {
            await _context.Videos.AddAsync(_mockVideo);
            await _context.SaveChangesAsync();

            await _tagRepository.AddTagsForVideoAsync(_mockTags, _mockVideo.Id);
            var result = await _tagRepository.GetAllAsync();

            Assert.Equal(3, result.Count);
            Assert.IsType<List<Tag>>(result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task AddTagsForVideoAsync_GivenInvalidVideoId_ThrowsArgumentOutOfRangeException(int id)
        {
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await _tagRepository.AddTagsForVideoAsync(new List<Tag>(), id));
        }
    }
}
