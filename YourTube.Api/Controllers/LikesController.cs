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
    public class LikesController : ControllerBase
    {
        private readonly ILogger<LikesController> _logger;
        private readonly ILikeRepository _likesRepository;
        private readonly IMapper _mapper;

        public LikesController(ILogger<LikesController> logger, ILikeRepository likesRepository, IMapper mapper)
        {
            _logger = logger;
            _likesRepository = likesRepository;
            _mapper = mapper;
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateLike(LikeDto likeDto)
        {
            try
            {
                _logger.LogInformation($"Adding like");

                if (likeDto == null)
                    return BadRequest();

                var like = _mapper.Map<Like>(likeDto);

                var createdLike = await _likesRepository.AddAsync(like);
                if (createdLike == null)
                    return NoContent();

                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to add like:", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
