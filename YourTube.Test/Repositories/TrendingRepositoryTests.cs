using Microsoft.EntityFrameworkCore;
using YourTube.Api.Data;
using YourTube.Api.Interfaces;
using YourTube.Api.Models;
using YourTube.Api.Repositories;

namespace YourTube.Test.Repositories
{
    public class TrendingRepositoryTests : IDisposable
    {
        ITrendingRepository _trendingRepository;
        YourTubeContext _context;
        DbContextOptions<YourTubeContext> _options = new DbContextOptionsBuilder<YourTubeContext>()
            .UseInMemoryDatabase("TrendingRepositoryTests")
            .Options;

        List<Video> _mockVideos = new List<Video>
        {
            new Video { Id = 1, Title = "test", User = new User { Id = 1 }, Comments = new List<Comment>() },
            new Video { Id = 2, Title = "test", User = new User { Id = 2 }, Comments = new List<Comment>() },
            new Video { Id = 3, Title = "test", User = new User { Id = 4 }, Comments = new List<Comment>() },
            new Video { Id = 4, Title = "test", User = new User { Id = 5 }, Comments = new List<Comment>() },
            new Video { Id = 5, Title = "test", User = new User { Id = 6 }, Comments = new List<Comment>() },
            new Video { Id = 6, Title = "test", User = new User { Id = 7 }, Comments = new List<Comment>() },
            new Video { Id = 7, Title = "test", User = new User { Id = 8 }, Comments = new List<Comment>() },
            new Video { Id = 8, Title = "test", User = new User { Id = 9 }, Comments = new List<Comment>() },
            new Video { Id = 9, Title = "test", User = new User { Id = 10 }, Comments = new List<Comment>() },
            new Video { Id = 10, Title = "test", User = new User { Id = 11 }, Comments = new List<Comment>() },
            new Video { Id = 11, Title = "test", User = new User { Id = 12 }, Comments = new List<Comment>() }
        };

        List<VideoView> _mockVideoViews = new List<VideoView>
        {
            new VideoView { Id = 1, VideoId = 1, Username = "test@gmail.com" },
            new VideoView { Id = 2, VideoId = 2, Username = "test@gmail.com" },
            new VideoView { Id = 3, VideoId = 2, Username = "test@gmail.com" },
            new VideoView { Id = 4, VideoId = 3, Username = "test@gmail.com" },
            new VideoView { Id = 5, VideoId = 4, Username = "test@gmail.com" },
            new VideoView { Id = 6, VideoId = 5, Username = "test@gmail.com" },
            new VideoView { Id = 7, VideoId = 6, Username = "test@gmail.com" },
            new VideoView { Id = 8, VideoId = 7, Username = "test@gmail.com" },
            new VideoView { Id = 9, VideoId = 8, Username = "test@gmail.com" },
            new VideoView { Id = 10, VideoId = 9, Username = "test@gmail.com" },
            new VideoView { Id = 11, VideoId = 10, Username = "test@gmail.com" },
            new VideoView { Id = 12, VideoId = 11, Username = "test@gmail.com" }
        };

        public TrendingRepositoryTests()
        {
            _context = new YourTubeContext(_options);

            _trendingRepository = new TrendingRepository(_context);

            _context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }

        [Fact]
        public async Task GetTrendingVideos()
        {
            await _context.Videos.AddRangeAsync(_mockVideos);
            await _context.VideoViews.AddRangeAsync(_mockVideoViews);
            await _context.SaveChangesAsync();

            var result = await _trendingRepository.GetTrendingVideos();

            Assert.Equal(10, result.Count());
            Assert.IsType<List<Video>>(result);
            Assert.Equal(2, result[0].Id);
        }
    }
}
