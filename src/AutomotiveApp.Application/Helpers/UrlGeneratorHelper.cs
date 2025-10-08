using AutomotiveApp.Base.Entities;
using AutomotiveApp.Application.Interfaces.Utils;

namespace AutomotiveApp.Application.Helpers
{
    public class UrlGeneratorHelper()
    {
        private const string WEB_URL = "http://localhost:5001/";
        private const string PUBLIC_PATH = "/images";
        private readonly string STORAGE_DIR = Path.Combine(Directory.GetCurrentDirectory(), "Storage", "Images");

        public string? GeneratePublicUrl<T>(string? filename) where T : BaseEntity
        {
            if (string.IsNullOrWhiteSpace(filename))
                return null;

            var filePath = Path.Combine(STORAGE_DIR, typeof(T).Name, filename);
            if (!File.Exists(filePath))
                return null;

            var relativePath = Path.Combine(PUBLIC_PATH.TrimStart('/'), typeof(T).Name, filename).Replace("\\", "/");
            return $"{WEB_URL}{relativePath}";
        }

    }
}
