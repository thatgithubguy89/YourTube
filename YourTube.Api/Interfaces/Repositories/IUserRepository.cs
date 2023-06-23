using YourTube.Api.Models;
using YourTube.Api.Models.Requests;

namespace YourTube.Api.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task AddUserAsync(SignupRequest signupRequest);
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> GetUserByUsernameWithoutVideosAsync(string username);
    }
}
