using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using InternshipChat.DAL.Data;
using InternshipChat.DAL.Entities;
using InternshipChat.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Xceed.Words.NET;

namespace InternshipChat.AttachmentFunctions
{
    public class ActivityFunctions
    {
        private readonly ChatContext _chatContext;
        private readonly BlobContainerClient _blobContainerClient;

        public ActivityFunctions(ChatContext chatContext)
        {
            var accountConnectionString = "";
            var containerName = "attachments-container";
            _blobContainerClient = new BlobContainerClient(accountConnectionString, containerName);
            _chatContext = chatContext;
        }

        [FunctionName(nameof(LoadFileToStorage))]
        public async Task<string> LoadFileToStorage([ActivityTrigger] FileModel inputFile, ILogger log)
        {
            Stream fileStream = new MemoryStream(inputFile.Content);
            var blob = _blobContainerClient.GetBlobClient(inputFile.FileName);

            await blob.UploadAsync(fileStream, true);

            return blob.Name;
        }

        [FunctionName(nameof(ExtractTextFromFile))]
        public async Task<string> ExtractTextFromFile([ActivityTrigger] string loadedBlobName, ILogger log)
        {
            var blobClient = _blobContainerClient.GetBlobClient(loadedBlobName);
            using MemoryStream stream = new();
            var downloadResponse = await blobClient.DownloadToAsync(stream);
            stream.Position = 0;

            if (downloadResponse.IsError)
            {
                return "";
            }

            using var docxReader = DocX.Load(stream);
            var extractedText = docxReader.Text;
            log.LogInformation("EXTRACTED TEXT: " + extractedText);
            return extractedText;
        }

        [FunctionName(nameof(SaveTextToDatabase))]
        public async Task<string> SaveTextToDatabase([ActivityTrigger] ChatAttachment chatAttachment, ILogger log)
        {
            await _chatContext.ChatAttachments.AddAsync(chatAttachment);
            await _chatContext.SaveChangesAsync();
            return "";
        }
    }
}