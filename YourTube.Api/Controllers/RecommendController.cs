using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using YourTube.Api.Interfaces.Services;
using YourTube.Api.Models;
using YourTube.Api.Models.Dtos;

namespace YourTube.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendController : ControllerBase
    {
        private readonly IRecommendService _recommendService;
        private readonly ILogger<RecommendController> _logger;
        private readonly IMapper _mapper;

        public RecommendController(IRecommendService recommendService, ILogger<RecommendController> logger, IMapper mapper)
        {
            _recommendService = recommendService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost("{videoId}")]
        [ProducesResponseType(typeof(List<VideoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetRecommendedVideos(int videoId, List<TagDto> tags)
        {
            try
            {
                _logger.LogInformation("Getting recommended videos");

                var videos = await _recommendService.GetRecommendedVideosAsync(videoId, _mapper.Map<List<Tag>>(tags));

                return Ok(_mapper.Map<List<VideoDto>>(videos));
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get recommended videos: ", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
