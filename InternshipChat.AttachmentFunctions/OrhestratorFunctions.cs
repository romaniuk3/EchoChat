using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using InternshipChat.Shared.DTO.ChatDtos;
using InternshipChat.DAL.Entities;
using Microsoft.Extensions.Logging;

namespace InternshipChat.AttachmentFunctions
{
    public class OrhestratorFunctions
    {
        [FunctionName(nameof(ProcessFileOrchestrator))]
        public async Task<ChatAttachment> ProcessFileOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context, ILogger log)
        {
            log = context.CreateReplaySafeLogger(log);
            var attachmentInput = context.GetInput<ChatAttachment>();
            log.LogInformation("ORCHESTRATOR");
            log.LogInformation("SENDER ID " + attachmentInput.SenderId);
            log.LogInformation("CHAT ID " + attachmentInput.ChatId);
            log.LogInformation("FILENAME " + attachmentInput.FileName);
            log.LogInformation("FILENAME FROM DOCUMENT" + attachmentInput.Attachment.FileName);
            //var loadedBlobName = await context.CallActivityAsync<string>("LoadFileToStorage", attachmentInput.Attachment);
            //var textFromBlob = await context.CallActivityAsync<string>("ExtractTextFromFile", loadedBlobName);
            //attachmentInput.FileText = textFromBlob;
            //var savedAttachment = await context.CallActivityAsync<ChatAttachment>("SaveTextToDatabase", attachmentInput);
            return new ChatAttachment();
            //return savedAttachment;
        }
    }
}
