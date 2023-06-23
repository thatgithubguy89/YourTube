using Microsoft.EntityFrameworkCore;
using YourTube.Api.Data;
using YourTube.Api.Interfaces.Repositories;
using YourTube.Api.Models;
using YourTube.Api.Repositories;

namespace YourTube.Test.Repositories
{
    public class FavoriteRepositoryTests : IDisposable
    {
        IFavoriteRepository _favoriteRepository;
        DbContextOptions<YourTubeContext> _options = new DbContextOptionsBuilder<YourTubeContext>()
            .UseInMemoryDatabase("FavoriteRepositoryTests")
            .Options;
        YourTubeContext _context;

        Video _mockVideo = new Video { Id = 1 };

        public FavoriteRepositoryTests()
        {
            _context = new YourTubeContext(_options);

            _favoriteRepository = new FavoriteRepository(_context);

            _context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }

        [Fact]
        public async Task AddAsync()
        {
            var favorite = new Favorite { Id = 1, Username = "test@gmail.com", VideoId = 1 };
            await _context.Videos.AddAsync(_mockVideo);
            await _context.SaveChangesAsync();

            await _favoriteRepository.AddAsync(favorite);
            var result = await _favoriteRepository.GetByIdAsync(1);

            Assert.Equal(1, result.Id);
            Assert.IsType<Favorite>(result);
        }

        [Fact]
        public async Task AddAsync_GivenUserHasAlreadySavedVideo_ReturnsNull()
        {
            var favorite = new Favorite { Id = 1, Username = "test@gmail.com", VideoId = 1 };
            await _context.Favorites.AddAsync(favorite);
            await _context.Videos.AddAsync(_mockVideo);
            await _context.SaveChangesAsync();

            var result = await _favoriteRepository.AddAsync(favorite);

            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_GivenInvalidFavorite_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _favoriteRepository.AddAsync(null));
        }

        [Fact]
        public async Task DeleteAsync()
        {
            var favorite = new Favorite { Id = 1, Username = "test@gmail.com", VideoId = 1 };
            await _context.Favorites.AddAsync(favorite);
            await _context.Videos.AddAsync(_mockVideo);
            await _context.SaveChangesAsync();

            await _favoriteRepository.DeleteAsync(favorite);
            var result = await _favoriteRepository.GetByIdAsync(1);

            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteAsync_GivenFavoriteThatDoesNoteExist_ThrowsNullReferenceException()
        {
            var favorite = new Favorite { Id = 1, Username = "test@gmail.com", VideoId = 1 };
            await _context.Videos.AddAsync(_mockVideo);
            await _context.SaveChangesAsync();

            await Assert.ThrowsAsync<NullReferenceException>(async () => await _favoriteRepository.DeleteAsync(favorite));
        }

        [Fact]
        public async Task DeleteAsync_GivenInvalidFavorite_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _favoriteRepository.DeleteAsync(null));
        }
    }
}
