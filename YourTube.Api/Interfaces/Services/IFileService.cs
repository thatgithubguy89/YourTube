namespace YourTube.Api.Interfaces.Services
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(string directory, IFormFile file);
        void DeleteFile(string path);
    }
}
