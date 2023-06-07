using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YourTube.Api.Data;
using YourTube.Api.Interfaces;
using YourTube.Api.Models;
using YourTube.Api.Models.Requests;
using YourTube.Api.Services;

namespace YourTube.Api.Repositories
{
    public class VideoRepository : Repository<Video>, IVideoRepository
    {
        private readonly YourTubeContext _context;
        private readonly IFileService _fileService;
        private readonly ICacheService<Video> _cacheService;
        private readonly IMapper _mapper;

        public VideoRepository(YourTubeContext context, IFileService fileService, ICacheService<Video> cacheService, IMapper mapper) : base(context)
        {
            _context = context;
            _fileService = fileService;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task AddVideoAsync(AddVideoRequest addVideoRequest)
        {
            if (addVideoRequest == null)
                throw new ArgumentNullException(nameof(addVideoRequest));

            var video = addVideoRequest.Video;
            video.VideoUrl = await _fileService.UploadFileAsync("videos", addVideoRequest.File);

            _cacheService.DeleteItems("all-videos");

            await _context.Videos.AddAsync(_mapper.Map<Video>(video));
            await _context.SaveChangesAsync();
        }

        public override async Task DeleteAsync(Video video)
        {
            if (video == null)
                throw new ArgumentNullException(nameof(video));

            _cacheService.DeleteItems("all-videos");
            _cacheService.DeleteItems($"video-{video.Id}");

            _fileService.DeleteFile(video.VideoUrl);

            _context.Videos.Remove(video);
            await _context.SaveChangesAsync();
        }

        public override async Task<List<Video>> GetAllAsync()
        {
            var videos = _cacheService.GetItems("all-videos");
            if (videos != null)
                return videos;

            videos = await _context.Videos.Include(v => v.User)
                                          .ToListAsync();

            _cacheService.SetItems("all-videos", videos);

            return videos;
        }

        public override async Task<Video> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));

            var video = _cacheService.GetItem($"video-{id}");
            if (video != null)
                return video;

            video = await _context.Videos.Include(v => v.Comments)
                                         .Include(v => v.User)
                                         .FirstOrDefaultAsync(v => v.Id == id);

            _cacheService.SetItem($"video-{id}", video);

            return video;
        }
    }
}
