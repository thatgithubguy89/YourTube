using Microsoft.EntityFrameworkCore;
using YourTube.Api.Data;
using YourTube.Api.Interfaces;
using YourTube.Api.Models;

namespace YourTube.Api.Repositories
{
    public class LikeRepository : Repository<Like>, ILikeRepository
    {
        private readonly YourTubeContext _context;
        private readonly IVideoRepository _videoRepository;

        public LikeRepository(YourTubeContext context, IVideoRepository videoRepository) : base(context)
        {
            _context = context;
            _videoRepository = videoRepository;
        }

        public override async Task<Like> AddAsync(Like like)
        {
            if (like == null)
                throw new ArgumentNullException(nameof(like));

            if (await HasUserLikedVideoAsync(like.Username, like.VideoId))
                return null;

            var video = await _videoRepository.GetByIdAsync(like.VideoId);
            if (video == null)
                throw new NullReferenceException();

            video.Liked += like.Liked ? 1 : 0;
            video.Disliked += like.Disliked ? 1 : 0;

            await _videoRepository.UpdateAsync(video);

            var createdLike = await _context.Likes.AddAsync(like);
            await _context.SaveChangesAsync();

            return createdLike.Entity;
        }

        private async Task<bool> HasUserLikedVideoAsync(string username, int videoId)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException(nameof(username));
            if (videoId <= 0)
                throw new ArgumentOutOfRangeException(nameof(videoId));

            var like = await _context.Likes.FirstOrDefaultAsync(l => l.Username == username && l.VideoId == videoId);

            return like != null;
        }
    }
}
