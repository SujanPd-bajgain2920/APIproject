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

        //  Save temp file inside project
        public async Task<string> SaveTempFileAsync(IFormFile file)
        {
            var tempFolder = Path.Combine(_env.WebRootPath, "TempUploads");

            if (!Directory.Exists(tempFolder))
                Directory.CreateDirectory(tempFolder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var tempPath = Path.Combine(tempFolder, fileName);

            using (var stream = new FileStream(tempPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // return relative path for session
            return Path.Combine("TempUploads", fileName);
        }

        //  Move file from temp to permanent
        public async Task<string> MoveToPermanentAsync(string tempPath, string folder, string fileName)
        {
            var rootPath = _env.WebRootPath;

            var sourcePath = Path.Combine(rootPath, tempPath);
            var destinationFolder = Path.Combine(rootPath, folder);

            if (!Directory.Exists(destinationFolder))
                Directory.CreateDirectory(destinationFolder);

            var destinationPath = Path.Combine(destinationFolder, fileName);

            if (File.Exists(sourcePath))
            {
                File.Copy(sourcePath, destinationPath, true);
                File.Delete(sourcePath); // cleanup temp
            }
            else
            {
                throw new FileNotFoundException("Temp file not found", sourcePath);
            }

            return Path.Combine(folder, fileName);
        }

        // optional cleanup
        public void DeleteTempFile(string tempPath)
        {
            var fullPath = Path.Combine(_env.WebRootPath, tempPath);

            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }
    }
}