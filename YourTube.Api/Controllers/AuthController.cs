using Microsoft.AspNetCore.Mvc;
using YourTube.Api.Models.Requests;
using YourTube.Api.Models.Responses;
using YourTube.Api.Interfaces.Repositories;
using YourTube.Api.Interfaces.Services;

namespace YourTube.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;

        public AuthController(ILogger<AuthController> logger, IUserRepository userRepository, IAuthService authService)
        {
            _logger = logger;
            _userRepository = userRepository;
            _authService = authService;
        }

        [HttpPost("signup")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Signup([FromForm] SignupRequest signupRequest)
        {
            try
            {
                _logger.LogInformation("Signing up user");

                if (signupRequest == null)
                    return BadRequest();

                var user = await _userRepository.GetUserByUsernameWithoutVideosAsync(signupRequest.Email);
                if (user != null)
                    return BadRequest();

                await _userRepository.AddUserAsync(signupRequest);

                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Failed to sign up user: ", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("signin")]
        [ProducesResponseType(typeof(SigninResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Signin(SigninRequest signinRequest)
        {
            try
            {
                _logger.LogInformation("Signing in user");

                if (signinRequest == null)
                    return BadRequest();

                var signinResponse = await _authService.AuthorizeUserAsync(signinRequest);
                if (signinResponse == null)
                    return Unauthorized();

                return Ok(signinResponse);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Failed to sign in user: ", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
