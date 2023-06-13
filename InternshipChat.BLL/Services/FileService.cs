using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using InternshipChat.BLL.Services.Contracts;
using InternshipChat.Shared.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.BLL.Services
{
    public class FileService : IFileService
    {
        private readonly string StorageConnectionString = string.Empty;
        private readonly string StorageAccessKey = string.Empty;
        private readonly string BlobContainerName = string.Empty;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileService(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;

            if (webHostEnvironment.IsProduction())
            {
                StorageConnectionString = configuration.GetSection("storageconnectionstring").Value!;
                StorageAccessKey = _configuration.GetSection("storageaccesskey").Value!;
                BlobContainerName = configuration["BlobContainerName"]!;
            }
        }

        public string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);

            return string.Concat(Path.GetFileNameWithoutExtension(fileName)
                , "_"
                , Guid.NewGuid().ToString().AsSpan(0, 10)
                , Path.GetExtension(fileName));
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            var fileName = GetUniqueFileName(file.FileName);
            BlobContainerClient blobContainer = new BlobContainerClient(StorageConnectionString, BlobContainerName);
            BlobClient client = blobContainer.GetBlobClient(fileName);
            await using (Stream stream = file.OpenReadStream())
            {
                await client.UploadAsync(stream);
            }

            return client.Uri.AbsoluteUri;
        }

        public async Task<UploadAttachmentResult> UploadDocumentAsync(FileModel fileModel, bool update = false)
        {
            string fileName = update ? fileModel.FileName : GetUniqueFileName(fileModel.FileName);

            var containerName = "attachments-container";

            BlobContainerClient blobContainer = new BlobContainerClient(StorageConnectionString, containerName);
            BlobClient client = blobContainer.GetBlobClient(fileName);
            Stream fileStream = new MemoryStream(fileModel.Content);
            await client.UploadAsync(fileStream, overwrite: update);

            return new UploadAttachmentResult
            {
                AttachmentUrl = client.Uri.AbsoluteUri,
                FileName = fileName
            };
        }

        public string GenerateSasTokenForBlob(string blobName, string? containerName = null)
        {
            var azureStorageAccount = _configuration["StorageAccountName"];
            var blobContainer = containerName != null ? containerName : BlobContainerName;

            var sasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = blobContainer,
                BlobName = blobName,
                Resource = "b",
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(1),
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);
            var sasToken = sasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(azureStorageAccount, StorageAccessKey)).ToString();

            return sasToken;
        }

        public string GenerateSasTokenForBlobContainer()
        {
            if (_webHostEnvironment.IsDevelopment()) return string.Empty;

            var azureStorageAccount = _configuration["StorageAccountName"];
            var sasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = BlobContainerName,
                Resource = "c",
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(2),
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);
            var sasToken = sasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(azureStorageAccount, StorageAccessKey)).ToString();

            return sasToken;
        }
    }
}
