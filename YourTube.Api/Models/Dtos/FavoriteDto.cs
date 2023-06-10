using YourTube.Api.Models.Common;

namespace YourTube.Api.Models.Dtos
{
    public class FavoriteDto : BaseEntity
    {
        public int VideoId { get; set; }
        public Video? Video { get; set; }
        public string? Username { get; set; }
    }
}
