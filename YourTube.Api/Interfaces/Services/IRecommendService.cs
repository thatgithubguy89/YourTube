using YourTube.Api.Models;

namespace YourTube.Api.Interfaces.Services
{
    public interface IRecommendService
    {
        Task<List<Video>> GetRecommendedVideosAsync(int videoId, List<Tag> tags);
    }
}
