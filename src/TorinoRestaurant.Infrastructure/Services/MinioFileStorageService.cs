using Microsoft.Extensions.Configuration;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using TorinoRestaurant.Application.Abstractions.Services;

namespace TorinoRestaurant.Infrastructure.Services
{
    public class MinIOFileStorageService : IFileStorageService
    {
        private readonly IMinioClient _minioClient;
        private readonly string _bucketName;
        private readonly string baseUrl;
        private readonly IConfiguration _configuration;

        public MinIOFileStorageService(
            IMinioClient minioClient,
            string bucketName,
            IConfiguration configuration)
        {
            _minioClient = minioClient;
            _bucketName = bucketName;
            _configuration = configuration;
            baseUrl = $"{_configuration.GetValue<string>("BlobStorage:Protocol")}{_configuration.GetValue<string>("BlobStorage:endpoint")}/{_bucketName}/";
        }

        public async Task UploadFileAsync(string fileName, Stream stream, string contentType = "")
        {
            var putObjectArgs = new PutObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(fileName)
                .WithObjectSize(-1)
                .WithStreamData(stream)
                .WithContentType(contentType);

            await _minioClient.PutObjectAsync(putObjectArgs);
        }

        public async Task<(Stream, string)> DownloadFileAsync(string fileName)
        {
            var fileStream = new MemoryStream();
            var getObjectArgs = new GetObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(fileName)
                .WithCallbackStream((stream) =>
                {
                    stream.CopyTo(fileStream);
                });

            var file = await _minioClient.GetObjectAsync(getObjectArgs);
            fileStream.Position = 0;
            return (fileStream, file.ContentType);
        }

        public async Task<bool> FileExistsAsync(string fileName)
        {
            try
            {
                var statObjectArgs = new StatObjectArgs()
                    .WithBucket(_bucketName)
                    .WithObject(fileName);
                await _minioClient.StatObjectAsync(statObjectArgs);
                return true;
            }
            catch (MinioException)
            {
                return false;
            }
        }

        public async Task DeleteFileAsync(List<string> fileNames)
        {
            var removeObjectArgs = new RemoveObjectsArgs()
                .WithBucket(_bucketName)
                .WithObjects(fileNames);
            await _minioClient.RemoveObjectsAsync(removeObjectArgs);
        }

        public async Task CopyFileAsync(string fileName, string newFileName)
        {
            var src = new CopySourceObjectArgs().WithBucket(_bucketName).WithObject(fileName);
            var agrs = new CopyObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(newFileName)
                .WithCopyObjectSource(src);

            await _minioClient.CopyObjectAsync(agrs);
        }

        public async Task DeleteFileUrlAsync(List<string> filePath)
        {
            try
            {
                if (filePath != null && filePath.Count > 0)
                {
                    filePath = filePath.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Replace(baseUrl, "")).Distinct().ToList();
                    var removeObjectArgs = new RemoveObjectsArgs()
                        .WithBucket(_bucketName)
                        .WithObjects(filePath);
                    await _minioClient.RemoveObjectsAsync(removeObjectArgs);   
                }
            }
            catch
            {
                return;
            }
        }

        public async Task<string> UploadFileGetUrlAsync(string fileName, Stream stream, string contentType = "")
        {
            var putObjectArgs = new PutObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(fileName)
                .WithObjectSize(-1)
                .WithStreamData(stream)
                .WithContentType(contentType);
            await _minioClient.PutObjectAsync(putObjectArgs);

            return $"{baseUrl}{fileName}";
        }

        public string GetBaseUrl()
        {
            return baseUrl;
        }
    }
}