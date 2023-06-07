using YourTube.Api.Models.Common;

namespace YourTube.Api.Models
{
    public class Like : BaseEntity
    {
        public bool Liked { get; set; }
        public bool Disliked { get; set; }
        public string? Username { get; set; }
        public int VideoId { get; set; }
        public Video? Video { get; set; }
    }
}
