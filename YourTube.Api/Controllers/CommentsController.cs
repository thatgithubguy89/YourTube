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
    public class CommentsController : ControllerBase
    {
        private readonly ILogger<CommentsController> _logger;
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public CommentsController(ILogger<CommentsController> logger, ICommentRepository commentRepository, IMapper mapper)
        {
            _logger = logger;
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateComment(CommentDto commentDto)
        {
            try
            {
                _logger.LogInformation($"Adding comment");

                if (commentDto == null)
                    return BadRequest();

                await _commentRepository.AddAsync(_mapper.Map<Comment>(commentDto));

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to add comment: ", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
