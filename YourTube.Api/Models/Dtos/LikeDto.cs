using YourTube.Api.Models.Common;

namespace YourTube.Api.Models.Dtos
{
    public class LikeDto : BaseEntity
    {
        public bool Liked { get; set; }
        public bool Disliked { get; set; }
        public string? Username { get; set; }
        public int VideoId { get; set; }
        public VideoDto? Video { get; set; }
    }
}
