using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using InternshipChat.Shared.DTO.ChatDtos;
using InternshipChat.DAL.Entities;
using InternshipChat.Shared.Models;
using System.Threading;
using DarkLoop.Azure.Functions.Authorize;

namespace InternshipChat.AttachmentFunctions
{
    public class HttpFunctions
    {
        [FunctionAuthorize]
        [FunctionName(nameof(AttachmentStarter))]
        public async Task<IActionResult> AttachmentStarter(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            TimeSpan timeout = TimeSpan.FromSeconds(30);
            TimeSpan retryInterval = TimeSpan.FromSeconds(1);
            var form = await req.ReadFormAsync();
            var file = form.Files.GetFile("file");
            var fileModel = new FileModel
            {
                FileName = form["FileName"],
                Content = FileModel.ReadFileContent(file)
            };

            var attachment = new ChatAttachment
            {
                ChatId = int.Parse(form["ChatId"]),
                SenderId = int.Parse(form["SenderId"]),
                FileName = form["FileName"],
                Attachment = fileModel
            };

            string instanceId = await starter.StartNewAsync("ProcessFileOrchestrator", null, attachment);

            return await starter.WaitForCompletionOrCreateCheckStatusResponseAsync(req, instanceId, timeout, retryInterval);
        }

        private static TimeSpan? GetTimeSpan(HttpRequest request, string queryParameterName)
        {
            string queryParameterStringValue = request.GetQueryParameterDictionary()[queryParameterName];
            if (string.IsNullOrEmpty(queryParameterStringValue))
            {
                return null;
            }

            return TimeSpan.FromSeconds(double.Parse(queryParameterStringValue));
        }
    }
}
