using Moq;
using YourTube.Api.Interfaces.Repositories;
using YourTube.Api.Interfaces.Services;
using YourTube.Api.Models;
using YourTube.Api.Services;

namespace YourTube.Test.Services
{
    public class RecommendServiceTests
    {
        IRecommendService _recommendService;
        Mock<IVideoRepository> _mockVideoRepository;

        static readonly List<Tag> _mockTags = new List<Tag>
        {
            new Tag {Id = 1, Name = "fun"},
            new Tag {Id = 2, Name = "food"},
            new Tag {Id = 3, Name = "wild"}
        };

        static readonly List<Tag> _mockTags1 = new List<Tag>
        {
            new Tag {Id = 1, Name = "cool"},
            new Tag {Id = 2, Name = "fun"},
            new Tag {Id = 3, Name = "nature"}
        };

        static readonly List<Tag> _mockTags2 = new List<Tag>
        {
            new Tag {Id = 1, Name = "food"},
            new Tag {Id = 2, Name = "cool"},
            new Tag {Id = 3, Name = "finance"}
        };

        static readonly List<Tag> _mockTags3 = new List<Tag>
        {
            new Tag {Id = 1, Name = "health"},
            new Tag {Id = 2, Name = "fit"},
            new Tag {Id = 3, Name = "sports"}
        };

        List<Video> _mockVideos = new List<Video>
        {
            new Video { Id = 1, Title = "test 1", User = new User { Id = 1 }, Comments = new List<Comment>(), Tags = _mockTags1 },
            new Video { Id = 2, Title = "test 2", User = new User { Id = 2 }, Comments = new List<Comment>(), Tags = _mockTags2 },
            new Video { Id = 3, Title = "test 3", User = new User { Id = 3 }, Comments = new List<Comment>(), Tags = _mockTags3 }
        };

        public RecommendServiceTests()
        {
            _mockVideoRepository = new Mock<IVideoRepository>();
            _mockVideoRepository.Setup(v => v.GetAllAsync()).Returns(Task.FromResult(_mockVideos));

            _recommendService = new RecommendService(_mockVideoRepository.Object);
        }

        [Fact]
        public async Task GetRecommendedVideosAsync()
        {
            var result = await _recommendService.GetRecommendedVideosAsync(_mockTags);

            Assert.Equal(2, result.Count);
            Assert.Equal(1, result[0].Id);
            Assert.Equal(2, result[1].Id);
            Assert.IsType<List<Video>>(result);
        }
    }
}
