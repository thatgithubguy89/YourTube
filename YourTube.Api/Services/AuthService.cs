using RestSharp;
using BCryptNet = BCrypt.Net.BCrypt;
using YourTube.Api.Models.Requests;
using YourTube.Api.Models.Responses;
using YourTube.Api.Interfaces;
using System.Text.Json;

namespace YourTube.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly IUserRepository _userRepository;

        public AuthService(IConfiguration configuration, IWebHostEnvironment environment, IUserRepository userRepository)
        {
            _configuration = configuration;
            _environment = environment;
            _userRepository = userRepository;
        }

        public async Task<SigninResponse> AuthorizeUserAsync(SigninRequest signinRequest)
        {
            if (signinRequest == null)
                throw new ArgumentNullException(nameof(signinRequest));

            var user = await _userRepository.GetUserByUsernameWithoutVideosAsync(signinRequest.Email);
            if (user == null)
                return null;

            if (!BCryptNet.Verify(signinRequest.Password, user.Password))
                return null;

            var signinResponse = new SigninResponse
            {
                Token = CreateToken(),
                UserImagePath = user.ProfileImageUrl ?? (_environment.WebRootPath + "/profileimages/stock.jpg")
            };

            return signinResponse;
        }

        private string CreateToken()
        {
            var client = new RestClient(_configuration["Authentication:TokenUrl"]);
            var request = new RestRequest(_configuration["Authentication:TokenUrl"], Method.Post);
            var body = new
            {
                client_id = _configuration["Authentication:ClientId"],
                client_secret = _configuration["Authentication:ClientSecret"],
                audience = _configuration["Authentication:Audience"],
                grant_type = _configuration["Authentication:GrantType"]
            };

            request.AddHeader("content-type", "application/json");
            request.AddJsonBody(body);

            var response = client.Execute(request);

            var token = JsonSerializer.Deserialize<TokenResponse>(response.Content);

            return token.access_token;
        }
    }
}
