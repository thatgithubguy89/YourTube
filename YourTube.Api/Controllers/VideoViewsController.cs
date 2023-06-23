using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YourTube.Api.Interfaces.Repositories;
using YourTube.Api.Models;
using YourTube.Api.Models.Dtos;

namespace YourTube.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoViewsController : ControllerBase
    {
        private readonly ILogger<VideoViewsController> _logger;
        private readonly IVideoViewRepository _videoViewRepository;
        private readonly IMapper _mapper;

        public VideoViewsController(ILogger<VideoViewsController> logger, IVideoViewRepository videoViewRepository, IMapper mapper)
        {
            _logger = logger;
            _videoViewRepository = videoViewRepository;
            _mapper = mapper;
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateVideoView(VideoViewDto videoViewDto)
        {
            try
            {
                _logger.LogInformation("Creating video view");

                if (videoViewDto == null)
                    return BadRequest();

                var videoView = _mapper.Map<VideoView>(videoViewDto);

                await _videoViewRepository.AddVideoViewAsync(videoView);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Failed to create video view");

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
