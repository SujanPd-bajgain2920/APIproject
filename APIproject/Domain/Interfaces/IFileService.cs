namespace APIproject.Domain.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveTempFileAsync(IFormFile file);
        Task<string> MoveToPermanentAsync(string tempPath, string folder, string fileName);
        void DeleteTempFile(string tempPath);
    }
}
