using YourTube.Api.Models;

namespace YourTube.Api.Interfaces
{
    public interface IVideoViewRepository : IRepository<VideoView>
    {
        Task AddVideoViewAsync(VideoView videoView);
    }
}
