using YourTube.Api.Interfaces;
using YourTube.Api.Repositories;
using YourTube.Api.Services;

namespace YourTube.Api.Extensions
{
    public static class RepositoriesAndServices
    {
        public static IServiceCollection AddRepositoriesAndServices(this IServiceCollection services)
        {
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ILikeRepository, LikeRepository>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IVideoRepository, VideoRepository>();
            services.AddScoped<IVideoViewRepository, VideoViewRepository>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped(typeof(ICacheService<>), typeof(CacheService<>));
            services.AddScoped<IFileService, FileService>();

            return services;
        }
    }
}
