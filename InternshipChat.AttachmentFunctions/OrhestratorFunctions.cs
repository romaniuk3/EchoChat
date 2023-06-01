using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.AttachmentFunctions
{
    public static class OrhestratorFunctions
    {
        [FunctionName(nameof(ProcessFileOrchestrator))]
        public static async Task<object> ProcessFileOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var fileLocation = context.GetInput<string>();

            var loadedBlob = await context.CallActivityAsync<string>("LoadFileToStorage", fileLocation);
            var textFromBlob = await context.CallActivityAsync<string>("ExtractTextFromFile", loadedBlob);

            return new
            {
                FileLocation = fileLocation,
                LoadedBlob = loadedBlob,
                TextFromBlob = textFromBlob
            };
        }
    }
}
