using YourTube.Api.Models.Common;

namespace YourTube.Api.Models
{
    public class User : BaseEntity
    {
        public User()
        {
            Likes = new HashSet<Like>();
            Videos = new HashSet<Video>();
        }

        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? ProfileImageUrl { get; set; }
        public ICollection<Like>? Likes { get; set; }
        public ICollection<Video>? Videos { get; set; }
    }
}
