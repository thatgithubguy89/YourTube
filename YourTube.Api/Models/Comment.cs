using YourTube.Api.Models.Common;

namespace YourTube.Api.Models
{
    public class Comment : BaseEntity
    {
        public string? Content { get; set; }
        public string? Username { get; set; }
        public int VideoId { get; set; }
        public Video? Video { get; set; }
    }
}
