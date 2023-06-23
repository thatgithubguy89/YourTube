using YourTube.Api.Models.Common;

namespace YourTube.Api.Models
{
    public class Video : BaseEntity
    {
        public Video()
        {
            Comments = new HashSet<Comment>();
            Likes = new HashSet<Like>();
            Tags = new HashSet<Tag>();
        }

        public string? Title { get; set; }
        public string? VideoUrl { get; set; }
        public int Liked { get; set; }
        public int Disliked { get; set; }
        public int Views { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Favorite>? Favorites { get; set; }
        public ICollection<Like>? Likes { get; set; }
        public ICollection<Tag>? Tags { get; set; }
    }
}
