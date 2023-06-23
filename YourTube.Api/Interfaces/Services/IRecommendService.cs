using YourTube.Api.Models;

namespace YourTube.Api.Interfaces.Services
{
    public interface IRecommendService
    {
        Task<List<Video>> GetRecommendedVideosAsync(List<Tag> tags);
    }
}
