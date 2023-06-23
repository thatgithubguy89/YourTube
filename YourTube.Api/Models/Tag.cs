using YourTube.Api.Models.Common;

namespace YourTube.Api.Models
{
    public class Tag : BaseEntity
    {
        public string? Name { get; set; }
        public int VideoId { get; set; }
        public Video? Video { get; set; }
    }
}
