using YourTube.Api.Data;
using YourTube.Api.Interfaces.Repositories;
using YourTube.Api.Interfaces.Services;
using YourTube.Api.Models;

namespace YourTube.Api.Repositories
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        private readonly YourTubeContext _context;
        private readonly ICacheService<Video> _cacheService;

        public CommentRepository(YourTubeContext context, ICacheService<Video> cacheService) : base(context)
        {
            _context = context;
            _cacheService = cacheService;
        }

        public override async Task<Comment> AddAsync(Comment comment)
        {
            if (comment == null)
                throw new ArgumentNullException(nameof(comment));

            _cacheService.DeleteItems($"video-{comment.VideoId}");

            var createdComment = await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();

            return createdComment.Entity;
        }

        public override async Task DeleteAsync(Comment comment)
        {
            if (comment == null)
                throw new ArgumentNullException(nameof(comment));

            _cacheService.DeleteItems($"video-{comment.VideoId}");

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }
    }
}
