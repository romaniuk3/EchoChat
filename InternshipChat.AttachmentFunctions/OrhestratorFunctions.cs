using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using InternshipChat.AttachmentFunctions.Models;

namespace InternshipChat.AttachmentFunctions
{
    public class OrhestratorFunctions
    {
        [FunctionName(nameof(ProcessFileOrchestrator))]
        public async Task<object> ProcessFileOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var attachmentInput = context.GetInput<Attachment>();
            var loadedBlob = await context.CallActivityAsync<string>("LoadFileToStorage", attachmentInput);
            var textFromBlob = await context.CallActivityAsync<string>("ExtractTextFromFile", attachmentInput);

            return null;
            /*
            return new
            {
                FileLocation = file,
                LoadedBlob = loadedBlob,
                TextFromBlob = textFromBlob
            };*/
        }
    }
}
