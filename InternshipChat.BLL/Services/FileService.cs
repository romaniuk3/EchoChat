using Azure.Storage.Blobs;
using InternshipChat.BLL.Services.Contracts;
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
        private readonly string BlobContainerName = string.Empty;

        public FileService(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            if (webHostEnvironment.IsProduction())
            {
                StorageConnectionString = configuration.GetSection("storageconnectionstring").Value!;
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
    }
}
