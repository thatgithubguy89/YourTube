using AutoMapper;
using YourTube.Api.Models;
using YourTube.Api.Models.Dtos;

namespace YourTube.Api.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Comment, CommentDto>().ReverseMap();
            CreateMap<Like, LikeDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Video, VideoDto>().ReverseMap();
            CreateMap<VideoView, VideoViewDto>().ReverseMap();
        }
    }
}
