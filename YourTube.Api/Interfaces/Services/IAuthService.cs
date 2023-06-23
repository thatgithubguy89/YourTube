using YourTube.Api.Models.Requests;
using YourTube.Api.Models.Responses;

namespace YourTube.Api.Interfaces.Services
{
    public interface IAuthService
    {
        Task<SigninResponse> AuthorizeUserAsync(SigninRequest signinRequest);
    }
}
