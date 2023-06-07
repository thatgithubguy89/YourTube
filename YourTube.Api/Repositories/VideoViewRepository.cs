using Microsoft.EntityFrameworkCore;
using YourTube.Api.Data;
using YourTube.Api.Interfaces;
using YourTube.Api.Models;

namespace YourTube.Api.Repositories
{
    public class VideoViewRepository : Repository<VideoView>, IVideoViewRepository
    {
        private readonly YourTubeContext _context;
        private readonly IVideoRepository _videoRepository;

        public VideoViewRepository(YourTubeContext context, IVideoRepository videoRepository) : base(context)
        {
            _context = context;
            _videoRepository = videoRepository;
        }

        public async Task AddVideoViewAsync(VideoView videoView)
        {
            var view = await _context.VideoViews.FirstOrDefaultAsync(v => v.Username == videoView.Username && v.VideoId == videoView.VideoId);

            if (view != null)
                return;

            await base.AddAsync(videoView);

            var video = await _videoRepository.GetByIdAsync(videoView.VideoId);
            if (video == null)
                throw new NullReferenceException(nameof(video));

            video.Views++;

            await _videoRepository.UpdateAsync(video);
        }
    }
}
