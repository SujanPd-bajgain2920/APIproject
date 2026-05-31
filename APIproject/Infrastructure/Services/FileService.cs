using APIproject.Domain.Interfaces;

namespace APIproject.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;

        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }

        // save uploaded file to system temp folder and return the temp path
        public async Task<string> SaveTempFileAsync(IFormFile file)
        {
            var tempPath = Path.Combine(Path.GetTempPath(), $"upload_{Guid.NewGuid()}");
            using var stream = File.Create(tempPath);
            await file.CopyToAsync(stream);
            return tempPath;
        }

        // move file from temp path to a permanent location and return the new path
        public async Task<string> MoveToPermanentAsync(string tempPath, string folder, string fileName)
        {
            var folderPath = Path.Combine(_env.WebRootPath, folder);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var destPath = Path.Combine(folderPath, fileName);

            using var source = File.OpenRead(tempPath);
            using var dest = File.Create(destPath);
            await source.CopyToAsync(dest);

            File.Delete(tempPath);

            return fileName;
        }

        // deletes temp file safely
        public void DeleteTempFile(string tempPath)
        {
            if (File.Exists(tempPath))
                File.Delete(tempPath);
        }
    }
}
