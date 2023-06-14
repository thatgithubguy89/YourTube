using YourTube.Api.Models;

namespace YourTube.Api.Interfaces
{
    public interface ITrendingRepository
    {
        Task<List<Video>> GetTrendingVideos();
    }
}
