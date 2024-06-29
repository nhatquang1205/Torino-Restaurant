namespace TorinoRestaurant.Application.Abstractions.Services
{
    public interface IFileStorageService
    {
        Task UploadFileAsync(string fileName, Stream stream, string contentType = "");
        Task<(Stream, string)> DownloadFileAsync(string fileName);
        Task<bool> FileExistsAsync(string fileName);
        Task DeleteFileAsync(List<string> fileNames);
        Task CopyFileAsync(string fileName, string newFileName);
        Task<string> UploadFileGetUrlAsync(string fileName, Stream stream, string contentType = "");
        Task DeleteFileUrlAsync(List<string> filePath);
        string GetBaseUrl();
    }
}