using Microsoft.AspNetCore.Mvc;
using YourTube.Api.Interfaces;
using YourTube.Api.Models.Dtos;
using YourTube.Api.Repositories;
using AutoMapper;

namespace YourTube.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrendingController : ControllerBase
    {
        private readonly ITrendingRepository _trendingRepository;
        private readonly ILogger<TrendingController> _logger;
        private readonly IMapper _mapper;

        public TrendingController(ITrendingRepository trendingRepository, ILogger<TrendingController> logger, IMapper mapper)
        {
            _trendingRepository = trendingRepository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<VideoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetTrendingVideos()
        {
            try
            {
                _logger.LogInformation("Getting trending videos");

                var videos = await _trendingRepository.GetTrendingVideos();

                return Ok(_mapper.Map<List<VideoDto>>(videos));
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Failed to get trending videos: ", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
