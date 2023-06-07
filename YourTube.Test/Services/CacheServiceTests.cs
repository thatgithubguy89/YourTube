using Microsoft.Extensions.Caching.Memory;
using YourTube.Api.Models;
using YourTube.Api.Services;

namespace YourTube.Test.Services
{
    public class CacheServiceTests : IDisposable
    {
        ICacheService<Video> _cacheService;
        IMemoryCache _memoryCache;

        Video _mockVideo = new Video { Id = 1, Title = "test", User = new User { Id = 1 }, Comments = new List<Comment>() };

        List<Video> _mockVideos = new List<Video>
        {
            new Video { Id = 1, Title = "test", User = new User { Id = 1 }, Comments = new List<Comment>() },
            new Video { Id = 2, Title = "test", User = new User { Id = 2 }, Comments = new List<Comment>() },
            new Video { Id = 3, Title = "test", User = new User { Id = 3 }, Comments = new List<Comment>() }
        };

        public CacheServiceTests()
        {
            _memoryCache = new MemoryCache(new MemoryCacheOptions());

            _cacheService = new CacheService<Video>(_memoryCache);
        }

        public void Dispose()
        {
            _memoryCache.Dispose();
        }

        [Fact]
        public void SetItems()
        {
            _cacheService.SetItems("videos", _mockVideos);
            var result = _cacheService.GetItems("videos");

            Assert.Equal(_mockVideos.Count, result.Count);
            Assert.IsType<List<Video>>(result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void SetItems_GivenInvalidKey_ThrowsArgumentNullException(string key)
        {
            Assert.Throws<ArgumentNullException>(() => _cacheService.SetItems(key, _mockVideos));
        }

        [Fact]
        public void SetItem()
        {
            _cacheService.SetItem("video", _mockVideo);
            var result = _cacheService.GetItem("video");

            Assert.Equal(_mockVideo.Id, result.Id);
            Assert.IsType<Video>(result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void SetItem_GivenInvalidKey_ThrowsArgumentNullException(string key)
        {
            Assert.Throws<ArgumentNullException>(() => _cacheService.SetItem(key, _mockVideo));
        }

        [Fact]
        public void DeleteItems()
        {
            _cacheService.SetItems("videos", _mockVideos);

            _cacheService.DeleteItems("videos");
            var result = _cacheService.GetItem("videos");

            Assert.Null(result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void DeleteItems_GivenInvalidKey_ThrowsArgumentNullException(string key)
        {
            Assert.Throws<ArgumentNullException>(() => _cacheService.DeleteItems(key));
        }

        [Fact]
        public void GetItem()
        {
            _cacheService.SetItem("video", _mockVideo);

            var result = _cacheService.GetItem("video");

            Assert.Equal(_mockVideo.Id, result.Id);
            Assert.IsType<Video>(result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void GetItem_GivenInvalidKey_ThrowsArgumentNullException(string key)
        {
            Assert.Throws<ArgumentNullException>(() => _cacheService.GetItem(key));
        }

        [Fact]
        public void GetItems()
        {
            _cacheService.SetItems("videos", _mockVideos);

            var result = _cacheService.GetItems("videos");

            Assert.Equal(_mockVideos.Count, result.Count);
            Assert.IsType<List<Video>>(result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void GetItems_GivenInvalidKey_ThrowsArgumentNullException(string key)
        {
            Assert.Throws<ArgumentNullException>(() => _cacheService.GetItems(key));
        }
    }
}
