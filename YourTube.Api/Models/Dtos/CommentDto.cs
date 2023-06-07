using YourTube.Api.Models.Common;

namespace YourTube.Api.Models.Dtos
{
    public class CommentDto : BaseEntity
    {
        public string? Content { get; set; }
        public string? Username { get; set; }
        public int VideoId { get; set; }
        public VideoDto? Video { get; set; }
    }
}
