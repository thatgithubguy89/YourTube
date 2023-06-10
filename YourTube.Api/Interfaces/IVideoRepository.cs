using YourTube.Api.Models;
using YourTube.Api.Models.Requests;

namespace YourTube.Api.Interfaces
{
    public interface IVideoRepository : IRepository<Video>
    {
        Task AddVideoAsync(AddVideoRequest addVideoRequest);
        Task<List<Video>> GetFavoriteVideosByUsernameAsync(string username);
    }
}
