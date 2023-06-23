using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using YourTube.Api.Controllers;
using YourTube.Api.Interfaces.Repositories;
using YourTube.Api.Models;
using YourTube.Api.Models.Dtos;
using YourTube.Api.Profiles;

namespace YourTube.Test.Controllers
{
    public class CommentsControllerTests
    {
        Mock<ICommentRepository> _mockCommentRepository;
        IMapper _mapper;
        Mock<ILogger<CommentsController>> _mockLogger;

        public CommentsControllerTests()
        {
            var config = new MapperConfiguration(c => c.AddProfile(typeof(MappingProfile)));
            _mapper = new Mapper(config);

            _mockLogger = new Mock<ILogger<CommentsController>>();

            _mockCommentRepository = new Mock<ICommentRepository>();
        }

        [Fact]
        public async Task CreateComment()
        {
            _mockCommentRepository.Setup(c => c.AddAsync(It.IsAny<Comment>()));
            var _commentsController = new CommentsController(_mockLogger.Object, _mockCommentRepository.Object, _mapper);

            var actionResult = await _commentsController.CreateComment(new CommentDto());
            var result = actionResult as NoContentResult;

            Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
        }

        [Fact]
        public async Task CreateComment_GivenInvalidComment_ReturnsBadRequest()
        {
            var _commentsController = new CommentsController(_mockLogger.Object, _mockCommentRepository.Object, _mapper);

            var actionResult = await _commentsController.CreateComment(null);
            var result = actionResult as BadRequestResult;

            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task CreateComment_Failure_ReturnsInternalServerError()
        {
            _mockCommentRepository.Setup(c => c.AddAsync(It.IsAny<Comment>())).Throws(new Exception());
            var _commentsController = new CommentsController(_mockLogger.Object, _mockCommentRepository.Object, _mapper);

            var actionResult = await _commentsController.CreateComment(new CommentDto());
            var result = actionResult as ObjectResult;

            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
        }
    }
}
