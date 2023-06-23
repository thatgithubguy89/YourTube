using YourTube.Api.Data;
using YourTube.Api.Interfaces;
using YourTube.Api.Models;

namespace YourTube.Api.Repositories
{
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        public TagRepository(YourTubeContext context) : base(context)
        { }

        public async Task AddTagsForVideoAsync(List<Tag> tags, int videoId)
        {
            if (videoId <= 0)
                throw new ArgumentOutOfRangeException(nameof(videoId));

            foreach (var tag in tags)
            {
                if (tag != null)
                {
                    tag.VideoId = videoId;
                    await base.AddAsync(tag);
                }
            }
        }
    }
}
