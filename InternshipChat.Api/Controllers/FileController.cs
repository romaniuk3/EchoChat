using InternshipChat.BLL.Services.Contracts;
using InternshipChat.DAL.Entities;
using InternshipChat.Shared.DTO.ChatDtos;
using InternshipChat.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InternshipChat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file)
        {
            var blobUrl = await _fileService.UploadImageAsync(file);
            return Ok(blobUrl);
        }

        [HttpPost]
        [Route("upload/document")]
        public async Task<IActionResult> UploadDocument(ChatAttachment chatAttachment)
        {
            await Console.Out.WriteLineAsync("Requires sign " + chatAttachment.RequiresSignature);
            await Console.Out.WriteLineAsync("SenderID " + chatAttachment.SenderId);
            await Console.Out.WriteLineAsync("ChatID " + chatAttachment.ChatId);
            await Console.Out.WriteLineAsync("ReceiverID " + chatAttachment.ReceiverId);
            await Console.Out.WriteLineAsync("ATTACHMENT FILENAME " + chatAttachment.Attachment.FileName);
            //var blobUrl = await _fileService.UploadDocumentAsync(file);
            return Ok();
        }
    }
}
