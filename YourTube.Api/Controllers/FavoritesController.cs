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
    public class FavoritesController : ControllerBase
    {
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IVideoRepository _videoRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<FavoritesController> _logger;

        public FavoritesController(IFavoriteRepository favoriteRepository, IVideoRepository videoRepository, IMapper mapper,
            ILogger<FavoritesController> logger)
        {
            _favoriteRepository = favoriteRepository;
            _videoRepository = videoRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [Authorize]
        [HttpGet("{username}")]
        [ProducesResponseType(typeof(VideoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetUsersFavoriteVideos(string username)
        {
            try
            {
                _logger.LogInformation($"Getting favorite videos for user {username ?? "unkown"}");

                if (string.IsNullOrWhiteSpace(username))
                    return BadRequest();

                var videos = await _videoRepository.GetFavoriteVideosByUsernameAsync(username);

                return Ok(_mapper.Map<List<VideoDto>>(videos));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get favorite videos for user {username ?? "unknown"}: ", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateFavorite(FavoriteDto favorite)
        {
            try
            {
                _logger.LogInformation($"Creating favorite");

                if (favorite == null)
                    return BadRequest();

                await _favoriteRepository.AddAsync(_mapper.Map<Favorite>(favorite));

                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to create favorite: ", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteFavorite(FavoriteDto favorite)
        {
            try
            {
                _logger.LogInformation($"Deleting favorite");

                if (favorite == null)
                    return BadRequest();

                await _favoriteRepository.DeleteAsync(_mapper.Map<Favorite>(favorite));

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to delete: ", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
