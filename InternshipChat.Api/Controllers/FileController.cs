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
        private readonly IChatService _chatService;

        public FileController(IFileService fileService, IChatService chatService)
        {
            _fileService = fileService;
            _chatService = chatService;
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
            var blobUrl = await _fileService.UploadDocumentAsync(chatAttachment.Attachment);
            chatAttachment.AttachmentUrl = blobUrl;
            var saveAttachmentResult = await _chatService.AddChatAttachment(chatAttachment);
            if (saveAttachmentResult.IsFailure)
            {
                return BadRequest();
            } 

            return Ok();
        }
    }
}
