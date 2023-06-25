using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YourTube.Api.Interfaces.Repositories;
using YourTube.Api.Models.Dtos;
using YourTube.Api.Models.Requests;

namespace YourTube.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        private readonly ILogger<VideosController> _logger;
        private readonly IMapper _mapper;
        private readonly IVideoRepository _videoRepository;

        public VideosController(ILogger<VideosController> logger, IMapper mapper, IVideoRepository videoRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _videoRepository = videoRepository;
        }

        [HttpGet("getall/{searchPhrase?}")]
        [ProducesResponseType(typeof(List<VideoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAll(string? searchPhrase)
        {
            try
            {
                _logger.LogInformation("Getting all videos");

                var videos = await _videoRepository.GetAllVideosAsync(searchPhrase);

                return Ok(_mapper.Map<List<VideoDto>>(videos));
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get all videos: ", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(VideoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetSingleVideo(int id)
        {
            try
            {
                _logger.LogInformation("Getting video");

                if (id <= 0)
                    return BadRequest();

                var video = await _videoRepository.GetByIdAsync(id);
                if (video == null)
                    return NotFound();

                return Ok(_mapper.Map<VideoDto>(video));
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get video: ", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateVideo([FromForm] AddVideoRequest addVideoRequest)
        {
            try
            {
                _logger.LogInformation("Adding video");

                if (addVideoRequest == null)
                    return BadRequest();

                await _videoRepository.AddVideoAsync(addVideoRequest);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to add video: ", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteVideo(int id)
        {
            try
            {
                _logger.LogInformation("Deleting video");

                if (id <= 0)
                    return BadRequest();

                var video = await _videoRepository.GetByIdAsync(id);
                if (video == null)
                    return NotFound();

                await _videoRepository.DeleteAsync(video);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to delete video: ", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
