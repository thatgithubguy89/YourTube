using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using YourTube.Api.Data;
using YourTube.Api.Interfaces;
using YourTube.Api.Models;
using YourTube.Api.Models.Dtos;
using YourTube.Api.Models.Requests;
using YourTube.Api.Profiles;
using YourTube.Api.Repositories;
using YourTube.Api.Services;

namespace YourTube.Test.Repositories
{
    [Collection("Sequential")]
    public class VideoRepositoryTests : IDisposable
    {
        IVideoRepository _videoRepository;
        Mock<ICacheService<Video>> _mockCacheService;
        Mock<IFileService> _mockFileService;
        Mock<ITagRepository> _mockTagRepository;
        YourTubeContext _context;
        IMapper _mapper;
        DbContextOptions<YourTubeContext> _options = new DbContextOptionsBuilder<YourTubeContext>()
            .UseInMemoryDatabase("VideoRepositoryTests")
            .Options;

        Video _mockVideo = new Video { Id = 1, Title = "test", User = new User { Id = 1 }, Comments = new List<Comment>() };

        IFormFile _mockFile = new FormFile(new MemoryStream(new byte[1]), 0, 1, "test", "test");

        List<Favorite> _mockFavorites = new List<Favorite>
        {
            new Favorite { Id = 1, VideoId = 1, Username = "test@gmail.com" },
            new Favorite { Id = 2, VideoId = 2, Username = "test@gmail.com" }
        };

        List<Video> _mockVideos = new List<Video>
        {
            new Video { Id = 1, Title = "test 1", User = new User { Id = 1 }, Comments = new List<Comment>() },
            new Video { Id = 2, Title = "test 2", User = new User { Id = 2 }, Comments = new List<Comment>() },
            new Video { Id = 3, Title = "test 3", User = new User { Id = 3 }, Comments = new List<Comment>() }
        };

        public VideoRepositoryTests()
        {
            _context = new YourTubeContext(_options);

            var config = new MapperConfiguration(c => c.AddProfile(new MappingProfile()));
            _mapper = new Mapper(config);

            _mockFileService = new Mock<IFileService>();
            _mockFileService.Setup(f => f.UploadFileAsync(It.IsAny<string>(), It.IsAny<IFormFile>())).Returns(Task.FromResult("test"));
            _mockFileService.Setup(f => f.DeleteFile(It.IsAny<string>()));

            _mockCacheService = new Mock<ICacheService<Video>>();
            _mockCacheService.Setup(c => c.DeleteItems(It.IsAny<string>()));
            _mockCacheService.Setup(c => c.SetItems(It.IsAny<string>(), It.IsAny<List<Video>>()));
            _mockCacheService.Setup(c => c.GetItems(It.IsAny<string>())).Returns((List<Video>)null);

            _mockTagRepository = new Mock<ITagRepository>();
            _mockTagRepository.Setup(t => t.AddTagsForVideoAsync(It.IsAny<List<Tag>>(), It.IsAny<int>()));

            _videoRepository = new VideoRepository(_context, _mockFileService.Object, _mockCacheService.Object, _mapper, _mockTagRepository.Object);

            _context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }

        [Fact]
        public async Task AddVideoAsync()
        {
            AddVideoRequest addVideoRequest = new AddVideoRequest
            {
                Video = new VideoDto { Id = 1, Title = "test", User = new UserDto { Id = 1 }, Comments = new List<CommentDto>() },
                File = _mockFile
            };
            await _videoRepository.AddVideoAsync(addVideoRequest);

            var result = await _videoRepository.GetByIdAsync(1);

            Assert.Equal(1, result.Id);
            Assert.IsType<Video>(result);
        }

        [Fact]
        public async Task AddVideoAsync_GivenInvalidVideo_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _videoRepository.AddVideoAsync(null));
        }

        [Fact]
        public async Task DeleteAsync()
        {
            await _videoRepository.AddAsync(_mockVideo);

            await _videoRepository.DeleteAsync(_mockVideo);
            var result = await _videoRepository.GetByIdAsync(_mockVideo.Id);

            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteAsync_GivenInvalidVideo_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _videoRepository.DeleteAsync(null));
        }

        [Fact]
        public async Task GetAllAsync()
        {
            await _context.AddRangeAsync(_mockVideos);
            await _context.SaveChangesAsync();

            var result = await _videoRepository.GetAllAsync();

            Assert.Equal(3, result.Count);
            Assert.IsType<List<Video>>(result);
        }

        [Fact]
        public async Task GetAllAsync_ReturnCachedVideos()
        {
            _mockCacheService.Setup(c => c.GetItems(It.IsAny<string>())).Returns(_mockVideos);

            var result = await _videoRepository.GetAllAsync();

            Assert.Equal(3, result.Count);
            Assert.IsType<List<Video>>(result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task GetAllVideosAsync_NoSearchPhrase(string searchPhrase)
        {
            await _context.AddRangeAsync(_mockVideos);
            await _context.SaveChangesAsync();

            var result = await _videoRepository.GetAllVideosAsync(searchPhrase);

            Assert.Equal(3, result.Count);
            Assert.IsType<List<Video>>(result);
        }

        [Fact]
        public async Task GetAllVideosAsync_WithSearchPhrase()
        {
            await _context.AddRangeAsync(_mockVideos);
            await _context.SaveChangesAsync();

            var result = await _videoRepository.GetAllVideosAsync("test 2");

            Assert.Single(result);
            Assert.Equal(2, result[0].Id);
            Assert.IsType<List<Video>>(result);
        }

        [Fact]
        public async Task GetByIdAsync()
        {
            await _videoRepository.AddAsync(_mockVideo);

            var result = await _videoRepository.GetByIdAsync(_mockVideo.Id);

            Assert.Equal(1, result.Id);
            Assert.IsType<Video>(result);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnCachedVideo()
        {
            _mockCacheService.Setup(c => c.GetItem(It.IsAny<string>())).Returns(_mockVideo);

            var result = await _videoRepository.GetByIdAsync(_mockVideo.Id);

            Assert.Equal(1, result.Id);
            Assert.IsType<Video>(result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetByIdAsync_GivenInvalidId_ThrowsArgumentOutOfRangeException(int id)
        {
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await _videoRepository.GetByIdAsync(id));
        }

        [Fact]
        public async Task GetFavoriteVideosByUsernameAsync()
        {
            await _context.Videos.AddRangeAsync(_mockVideos);
            await _context.Favorites.AddRangeAsync(_mockFavorites);
            await _context.SaveChangesAsync();

            var result = await _videoRepository.GetFavoriteVideosByUsernameAsync("test@gmail.com");

            Assert.Equal(2, result.Count);
            Assert.IsType<List<Video>>(result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task GetFavoriteVideosByUsernameAsync_GivenInvalidUsername_ThrowsArgumentNullException(string username)
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _videoRepository.GetFavoriteVideosByUsernameAsync(username));
        }
    }
}
