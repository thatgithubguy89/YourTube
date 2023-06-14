using Microsoft.EntityFrameworkCore;
using YourTube.Api.Data;
using YourTube.Api.Interfaces;
using YourTube.Api.Models;

namespace YourTube.Api.Repositories
{
    public class TrendingRepository : ITrendingRepository
    {
        private readonly YourTubeContext _context;

        public TrendingRepository(YourTubeContext context)
        {
            _context = context;
        }

        public async Task<List<Video>> GetTrendingVideos()
        {
            var videos = new List<Video>();

            var ids = await _context.VideoViews.Where(v => v.CreateTime.Date.Day == DateTime.Now.Day)
                                               .GroupBy(v => v.VideoId)
                                               .OrderByDescending(v => v.Count())
                                               .Take(10)
                                               .Select(i => i.Key)
                                               .ToListAsync();

            foreach (var id in ids)
            {
                var video = await _context.Videos.Include(v => v.User)
                                                 .FirstOrDefaultAsync(v => v.Id == id);

                if (video != null)
                    videos.Add(video);
            }

            return videos;
        }
    }
}
