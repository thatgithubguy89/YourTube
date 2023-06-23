using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using YourTube.Api.Interfaces.Services;
using YourTube.Api.Services;

namespace YourTube.Test.Services
{
    public class FileServiceTests : IDisposable
    {
        IFileService _fileService;
        Mock<IWebHostEnvironment> _mockEnvironment;

        IFormFile _mockFile = new FormFile(new MemoryStream(new byte[1]), 0, 1, "test", "test");

        public FileServiceTests()
        {
            _mockEnvironment = new Mock<IWebHostEnvironment>();
            _mockEnvironment.Setup(e => e.WebRootPath).Returns("C:\\Projects\\YourTube\\YourTube.Test");

            _fileService = new FileService(_mockEnvironment.Object);

            Directory.CreateDirectory("C:\\Projects\\YourTube\\YourTube.Test\\test");
        }

        public void Dispose()
        {
            Directory.Delete("C:\\Projects\\YourTube\\YourTube.Test\\test", true);
        }

        [Fact]
        public async Task UploadFileAsync()
        {
            var result = await _fileService.UploadFileAsync("test", _mockFile);

            Assert.True(File.Exists($"C:\\Projects\\YourTube\\YourTube.Test\\{result}"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("test")]
        public async Task UploadFileAsync_GivenInvalidFile_ThrowsArgumentNullException(string directory)
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _fileService.UploadFileAsync(directory, null));
        }

        [Fact]
        public async Task DeleteFile()
        {
            var path = await _fileService.UploadFileAsync("test", _mockFile);

            _fileService.DeleteFile(path);

            Assert.False(File.Exists($"C:\\Projects\\YourTube\\YourTube.Test\\{path}"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void DeleteFile_GivenInvalidFile_ThrowsArgumentNullException(string path)
        {
            Assert.Throws<ArgumentNullException>(() => _fileService.DeleteFile(path));
        }
    }
}
