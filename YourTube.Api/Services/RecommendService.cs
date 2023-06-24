using YourTube.Api.Interfaces.Repositories;
using YourTube.Api.Interfaces.Services;
using YourTube.Api.Models;

namespace YourTube.Api.Services
{
    public class RecommendService : IRecommendService
    {
        private readonly IVideoRepository _videoRepository;

        public RecommendService(IVideoRepository videoRepository)
        {
            _videoRepository = videoRepository;
        }

        public async Task<List<Video>> GetRecommendedVideosAsync(int videoId, List<Tag> tags)
        {
            var tagNames = tags.ConvertAll(x => x.Name);

            var videos = await _videoRepository.GetAllAsync();

            videos = videos.Where(v => v.Tags.Any(t => tagNames.Contains(t.Name)) && v.Id != videoId)
                           .OrderByDescending(v => v.Views)
                           .Take(10)
                           .ToList();

            return videos;
        }
    }
}
