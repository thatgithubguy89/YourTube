using YourTube.Api.Models;

namespace YourTube.Api.Interfaces.Repositories
{
    public interface ITagRepository : IRepository<Tag>
    {
        Task AddTagsForVideoAsync(List<Tag> tags, int videoId);
    }
}
