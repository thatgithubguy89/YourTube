using YourTube.Api.Models;

namespace YourTube.Api.Interfaces.Repositories
{
    public interface ITrendingRepository
    {
        Task<List<Video>> GetTrendingVideos();
    }
}
