using System.ComponentModel.DataAnnotations.Schema;
using YourTube.Api.Models.Common;

namespace YourTube.Api.Models.Dtos
{
    public class VideoDto : BaseEntity
    {
        public VideoDto()
        {
            Comments = new HashSet<CommentDto>();
            Tags = new HashSet<TagDto>();
        }

        public string? Title { get; set; }
        public string? VideoUrl { get; set; }
        public int Liked { get; set; }
        public int Disliked { get; set; }
        public int Views { get; set; }
        public int UserId { get; set; }
        public UserDto? User { get; set; }
        public ICollection<CommentDto>? Comments { get; set; }
        public ICollection<TagDto>? Tags { get; set; }
    }
}
