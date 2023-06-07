using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YourTube.Api.Interfaces;
using YourTube.Api.Models;
using YourTube.Api.Models.Dtos;

namespace YourTube.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UsersController(ILogger<UsersController> logger, IMapper mapper, IUserRepository userRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        [Authorize]
        [HttpGet("{username}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetSingleUser(string username)
        {
            try
            {
                _logger.LogInformation($"Getting user {username}");

                if (string.IsNullOrWhiteSpace(username))
                    return BadRequest();

                var user = await _userRepository.GetUserByUsernameAsync(username);
                if (user == null)
                    return NotFound();

                return Ok(_mapper.Map<UserDto>(user));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get user {username}: ", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting user with the id of {id}");

                if (id <= 0)
                    return BadRequest();

                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                    return NotFound();

                await _userRepository.DeleteAsync(user);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to delete user with the id of {id}", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
