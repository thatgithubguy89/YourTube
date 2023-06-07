using YourTube.Api.Models.Common;

namespace YourTube.Api.Models.Dtos
{
    public class UserDto : BaseEntity
    {
        public UserDto()
        {
            Videos = new HashSet<VideoDto>();
        }

        public string? Username { get; set; }
        public string? ProfileImageUrl { get; set; }
        public ICollection<VideoDto>? Videos { get; set; }
    }
}
