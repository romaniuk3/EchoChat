using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace InternshipChat.AttachmentFunctions
{
    public static class ActivityFunctions
    {
        [FunctionName(nameof(LoadFileToStorage))]
        public static async Task<string> LoadFileToStorage([ActivityTrigger] string inputFile, ILogger log)
        {
            log.LogInformation($"File {inputFile} is being loaded.");
            await Task.Delay(2000);
            return $"Loaded {inputFile}.docx!";
        }

        [FunctionName(nameof(ExtractTextFromFile))]
        public static async Task<string> ExtractTextFromFile([ActivityTrigger] string inputFile, ILogger log)
        {
            log.LogInformation($"Test is ASDDASDSADASD");
            await Task.Delay(2000);

            return $"Extracted text is: ASDDASDSADASD";
        }
    }
}