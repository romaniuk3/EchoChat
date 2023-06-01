using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using InternshipChat.AttachmentFunctions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Xceed.Words.NET;

namespace InternshipChat.AttachmentFunctions
{
    public static class ActivityFunctions
    {
        [FunctionName(nameof(LoadFileToStorage))]
        public static async Task<string> LoadFileToStorage([ActivityTrigger] Attachment inputFile, ILogger log)
        {
            var accountConnectionString = "f";
            var containerName = "attachments-container";
            Stream fileStream = new MemoryStream(inputFile.Content);

            var blobClient = new BlobContainerClient(accountConnectionString, containerName);
            var blob = blobClient.GetBlobClient(inputFile.FileName);

            await blob.UploadAsync(fileStream, true);

            return blobClient.Uri.AbsoluteUri;
        }

        [FunctionName(nameof(ExtractTextFromFile))]
        public static async Task<string> ExtractTextFromFile([ActivityTrigger] Attachment inputFile, ILogger log)
        {
            using MemoryStream stream = new(inputFile.Content);
            var extractedText = string.Empty;

            using var docxReader = DocX.Load(stream);
            extractedText = docxReader.Text;

            return extractedText;
        }

        [FunctionName(nameof(SaveTextToDatabase))]
        public static async Task<string> SaveTextToDatabase([ActivityTrigger] string textFromFile, ILogger log)
        {
            
            
            return textFromFile;
        }
    }
}