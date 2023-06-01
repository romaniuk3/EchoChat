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
using InternshipChat.AttachmentFunctions.Models;

namespace InternshipChat.AttachmentFunctions
{
    public class HttpFunctions
    {
        [FunctionName(nameof(AttachmentStarter))]
        public async Task<IActionResult> AttachmentStarter(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            var file = req.Form.Files.FirstOrDefault();

            if (file == null)
            {
                return new BadRequestObjectResult("Please provide a valid .docx file");
            }
            var fileModel = new Attachment()
            {
                FileName = file.FileName,
                Content = Attachment.ReadFileContent(file)
            };

            log.LogInformation("PASSED FILENAME IS: " + fileModel.FileName);

            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("ProcessFileOrchestrator", null, fileModel);

            //log.LogInformation($"Started orchestration with ID = '{instanceId}'.");
            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}
