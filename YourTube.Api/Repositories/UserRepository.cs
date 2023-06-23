using Microsoft.EntityFrameworkCore;
using YourTube.Api.Data;
using YourTube.Api.Interfaces.Repositories;
using YourTube.Api.Interfaces.Services;
using YourTube.Api.Models;
using YourTube.Api.Models.Requests;
using BCryptNet = BCrypt.Net.BCrypt;

namespace YourTube.Api.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly YourTubeContext _context;
        private readonly IFileService _fileService;

        public UserRepository(YourTubeContext context, IFileService fileService) : base(context)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task AddUserAsync(SignupRequest signupRequest)
        {
            if (signupRequest == null)
                throw new ArgumentNullException(nameof(signupRequest));

            var user = new User
            {
                Username = signupRequest.Email,
                Password = BCryptNet.HashPassword(signupRequest.Password),
                ProfileImageUrl = await _fileService.UploadFileAsync("profileimages", signupRequest.File)
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public override async Task DeleteAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _fileService.DeleteFile(user.ProfileImageUrl);

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException(nameof(username));

            return await _context.Users.Where(u => u.Username == username)
                                               .Include(u => u.Videos)
                                               .FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByUsernameWithoutVideosAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException(nameof(username));

            return await _context.Users.Where(u => u.Username == username)
                                           .FirstOrDefaultAsync();
        }
    }
}
