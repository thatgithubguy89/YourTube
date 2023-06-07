using YourTube.Api.Models.Dtos;

namespace YourTube.Api.Models.Requests
{
    public class AddVideoRequest
    {
        public VideoDto Video { get; set; }
        public IFormFile File { get; set; }
    }
}
