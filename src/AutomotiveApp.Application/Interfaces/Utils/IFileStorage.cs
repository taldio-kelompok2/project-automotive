using AutomotiveApp.Base.Entities;

namespace AutomotiveApp.Application.Interfaces.Utils
{
    public interface IFileStorage
    {
        string GetFilePath<T>(string filename) where T : BaseEntity;
        Task<bool> SaveFileAsync<T>(Stream data, string filename) where T : BaseEntity;
        Task<Stream> ReadFileAsync<T>(string filename) where T : BaseEntity;
        Task DeleteFileAsync<T>(string filename) where T : BaseEntity;
        Task<bool> ReplaceFileAsync<T>(string destFile, Stream data) where T : BaseEntity;

    }
}