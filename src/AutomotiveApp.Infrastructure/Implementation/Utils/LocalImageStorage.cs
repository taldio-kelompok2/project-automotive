using AutomotiveApp.Base.Entities;
using AutomotiveApp.Application.Interfaces.Utils;
using Microsoft.Extensions.Logging;
namespace AutomotiveApp.Infrastructure.Implementation.Utils
{
    public class LocalImageStorage : IFileStorage
    {
        private readonly string STORAGE_DIR = Path.Combine(Directory.GetCurrentDirectory(), "Storage", "Images");
        private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".svg"];
        private readonly ILogger<LocalImageStorage> _logger;

        public LocalImageStorage(ILogger<LocalImageStorage> logger)
        {
            _logger = logger;

            if (!Directory.Exists(STORAGE_DIR))
            {
                Directory.CreateDirectory(STORAGE_DIR);
                _logger.LogInformation("Created root storage directory: {StorageDir}", STORAGE_DIR);
            }
        }

        private string GetSubDirPath<T>() where T : BaseEntity
        {
            string path = Path.Combine(STORAGE_DIR, typeof(T).Name);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                _logger.LogInformation("Created subdirectory for {Entity}: {Path}", typeof(T).Name, path);
            }
            return path;
        }

        public string GetFilePath<T>(string filename) where T : BaseEntity
        {
            return Path.Combine(GetSubDirPath<T>(), filename);
        }

        public async Task<bool> SaveFileAsync<T>(Stream data, string filename) where T : BaseEntity
        {
            if (data == null || data.Length == 0)
            {
                _logger.LogWarning("File stream is null or empty for {Filename}", filename);
                throw new ArgumentException("File data is empty", nameof(data));
            }

            var extension = Path.GetExtension(filename)?.ToLower();
            if (extension == null || !AllowedExtensions.Contains(extension))
            {
                _logger.LogWarning("Invalid file extension: {Extension}", extension);
                throw new InvalidOperationException("Invalid file extension.");
            }

            string path = GetFilePath<T>(filename);

            try
            {
                _logger.LogInformation("Saving file to {Path}", path);
                using FileStream filestream = new FileStream(path, FileMode.Create, FileAccess.Write);
                await data.CopyToAsync(filestream);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving file {Path}", path);
                throw;
            }

            bool exists = File.Exists(path);
            _logger.LogInformation("File saved: {Exists}", exists);
            return exists;
        }

        public Task<Stream> ReadFileAsync<T>(string filename) where T : BaseEntity
        {
            string path = GetFilePath<T>(filename);
            if (!File.Exists(path))
            {
                _logger.LogWarning("File not found: {Path}", path);
                throw new FileNotFoundException("File not found", path);
            }

            _logger.LogInformation("Reading file: {Path}", path);
            Stream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            return Task.FromResult(fileStream);
        }

        public Task DeleteFileAsync<T>(string filename) where T : BaseEntity
        {
            string path = GetFilePath<T>(filename);
            if (File.Exists(path))
            {
                File.Delete(path);
                _logger.LogInformation("Deleted file: {Path}", path);
            }
            else
            {
                _logger.LogWarning("File not found for deletion: {Path}", path);
            }

            return Task.CompletedTask;
        }

        public async Task<bool> ReplaceFileAsync<T>(string filename, Stream data) where T : BaseEntity
        {
            if (data == null || data.Length == 0)
            {
                _logger.LogWarning("File stream is null or empty for {Filename}", filename);
                throw new ArgumentException("File data is empty", nameof(data));
            }

            var extension = Path.GetExtension(filename)?.ToLower();
            if (extension == null || !AllowedExtensions.Contains(extension))
            {
                _logger.LogWarning("Invalid file extension: {Extension}", extension);
                throw new InvalidOperationException("Invalid file extension.");
            }

            var dir = Path.GetDirectoryName(GetFilePath<T>(filename));
            if (string.IsNullOrEmpty(dir))
                throw new InvalidOperationException($"Directory {dir} dosent exist");

            var fileBaseName = Path.GetFileNameWithoutExtension(filename);
            var oldFilePath = Directory.GetFiles(dir, fileBaseName + ".*").FirstOrDefault();

            try
            {
                if (File.Exists(oldFilePath))
                {
                    File.Delete(oldFilePath);
                    _logger.LogInformation("Deleted existing file: {Path}", oldFilePath);
                }

                var newFilePath = Path.Combine(dir, filename);
                using FileStream fs = new FileStream(newFilePath, FileMode.Create, FileAccess.Write);
                await data.CopyToAsync(fs);

                _logger.LogInformation("Replaced file successfully: {NewFilePath}", newFilePath);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error replacing file {File}", filename);
                return false;
            }
        }
    }
}
