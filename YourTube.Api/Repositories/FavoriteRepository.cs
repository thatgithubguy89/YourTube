using Microsoft.EntityFrameworkCore;
using YourTube.Api.Data;
using YourTube.Api.Interfaces.Repositories;
using YourTube.Api.Models;

namespace YourTube.Api.Repositories
{
    public class FavoriteRepository : Repository<Favorite>, IFavoriteRepository
    {
        private readonly YourTubeContext _context;

        public FavoriteRepository(YourTubeContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<Favorite> AddAsync(Favorite favorite)
        {
            if (favorite == null)
                throw new ArgumentNullException(nameof(favorite));

            if (await HasUserSavedVideo(favorite))
                return null;

            var createdFavorite = await _context.Favorites.AddAsync(favorite);
            await _context.SaveChangesAsync();

            return createdFavorite.Entity;
        }

        public override async Task DeleteAsync(Favorite favorite)
        {
            if (favorite == null)
                throw new ArgumentNullException(nameof(favorite));

            var favoriteToDelete = await _context.Favorites.Where(f => f.VideoId == favorite.VideoId && f.Username == favorite.Username)
                                                           .FirstOrDefaultAsync();

            if (favoriteToDelete == null)
                throw new NullReferenceException(nameof(favoriteToDelete));

            _context.Favorites.Remove(favoriteToDelete);
            await _context.SaveChangesAsync();
        }

        private async Task<bool> HasUserSavedVideo(Favorite favorite)
        {
            var favoriteCheck = await _context.Favorites.Where(f => f.VideoId == favorite.VideoId && f.Username == favorite.Username)
                                                           .FirstOrDefaultAsync();

            return favoriteCheck != null;
        }
    }
}
