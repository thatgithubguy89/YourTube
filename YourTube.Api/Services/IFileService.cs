namespace YourTube.Api.Services
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(string directory, IFormFile file);
        void DeleteFile(string path);
    }
}
