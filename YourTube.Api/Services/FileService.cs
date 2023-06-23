using YourTube.Api.Interfaces.Services;

namespace YourTube.Api.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;

        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> UploadFileAsync(string directory, IFormFile file)
        {
            if (string.IsNullOrWhiteSpace(directory))
                throw new ArgumentNullException(nameof(directory));
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            var fileName = Guid.NewGuid().ToString() + file.FileName;
            var path = Path.Combine(_environment.WebRootPath, $"{directory}/", fileName);

            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/{directory}/" + fileName;
        }

        public void DeleteFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path));

            var newPath = _environment.WebRootPath + path;

            if (File.Exists(newPath))
                File.Delete(newPath);
        }
    }
}
