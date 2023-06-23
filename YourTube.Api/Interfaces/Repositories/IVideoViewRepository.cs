using YourTube.Api.Models;

namespace YourTube.Api.Interfaces.Repositories
{
    public interface IVideoViewRepository : IRepository<VideoView>
    {
        Task AddVideoViewAsync(VideoView videoView);
    }
}
