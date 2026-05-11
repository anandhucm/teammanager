
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using MYTEAMMANAGER.OptionPatterns;
using Microsoft.Extensions.Options;

namespace MYTEAMMANAGER.Services
{

    public class UploadResult
    {
        public string Status { get; set; }
        public string Message { get; set; }
    }

    public interface IBlobService
    {
        Task<UploadResult>UploadAsync(IFormFile file, string id, string folder = "photos");
        Task<Object> DeletePhotos(string blobUrl);
        
    }
    public class BlobService : IBlobService
    {

        private readonly AzureBlobSettings _settings;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;

        public BlobService(IOptions<AzureBlobSettings> options)
        {
            // _blobServiceClient = new BlobServiceClient(
            //     config["AzureBlobStorage:ConnectionString"]
                
            // );
            // _containerName = config["AzureBlobStorage:ContainerName"]!;

            _settings = options.Value;
  
        }
        public async Task<UploadResult> UploadAsync(IFormFile file, string id, string folder = "photos")
        {
            // var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var _blobServiceClientFromOption = new BlobServiceClient(
                _settings.ConnectionString
                
            );
            var containerClient = _blobServiceClientFromOption.GetBlobContainerClient(_settings.ContainerName);

            Guid.TryParse(id, out Guid guidId);

            // Creates container if it doesn't exist, with public read access
            // await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            // Unique file name to avoid overwrites
            var extension  = Path.GetExtension(file.FileName);
            var blobName   = $"{folder}/{guidId}{extension}";
            var blobClient = containerClient.GetBlobClient(blobName);

            var blobHttpHeaders = new BlobHttpHeaders
            {
                ContentType = file.ContentType  // preserves file type (image/jpeg etc.)
            };

            await using var stream = file.OpenReadStream();
            var response = await blobClient.UploadAsync(stream, new BlobUploadOptions
            {
                HttpHeaders = blobHttpHeaders
            });

            if (response.GetRawResponse().Status == 201)
            {
                return new UploadResult
                    {
                        Status = "success",
                        Message = blobClient.Uri.ToString()
                    };
            }
            else
            {
                return new UploadResult
                    {
                        Status = "error",
                        Message = "Error occurred while uploading"
                    };
            }

        }

        public async Task<Object> DeletePhotos(string blobUrl)
        {
            return "";
            
        }

    }
}