using YourTube.Api.Models.Common;

namespace YourTube.Api.Models.Dtos
{
    public class TagDto : BaseEntity
    {
        public string? Name { get; set; }
        public int VideoId { get; set; }
        public VideoDto? Video { get; set; }
    }
}
