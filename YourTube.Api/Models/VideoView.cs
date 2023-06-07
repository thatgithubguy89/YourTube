using YourTube.Api.Models.Common;

namespace YourTube.Api.Models
{
    public class VideoView : BaseEntity
    {
        public int VideoId { get; set; }
        public string Username { get; set; }
    }
}
