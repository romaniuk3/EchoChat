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

namespace InternshipChat.AttachmentFunctions
{
    public static class HttpFunctions
    {
        [FunctionName(nameof(AttachmentStarter))]
        public static async Task<IActionResult> AttachmentStarter(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            var file = req.GetQueryParameterDictionary()["file"];

            if (file == null)
            {
                return new BadRequestObjectResult("Please pass the file location to query stirng");
            }

            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("ProcessFileOrchestrator", null, file);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}
