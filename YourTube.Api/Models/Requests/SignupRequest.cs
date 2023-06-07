namespace YourTube.Api.Models.Requests
{
    public class SignupRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public IFormFile File { get; set; }
    }
}
