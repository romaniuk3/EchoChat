using InternshipChat.Api.Extensions;
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
            var uploadResult = await _fileService.UploadDocumentAsync(chatAttachment.Attachment);
            chatAttachment.AttachmentUrl = uploadResult.AttachmentUrl;
            chatAttachment.FileName = uploadResult.FileName;
            var saveAttachmentResult = await _chatService.AddChatAttachment(chatAttachment);
            if (saveAttachmentResult.IsFailure)
            {
                return BadRequest();
            } 

            return Ok();
        }

        [HttpPut]
        [Route("document/{id}")]
        public async Task<IActionResult> UpdateDocument(int id, [FromBody] ChatAttachment chatAttachment)
        {
            chatAttachment.Attachment.FileName = chatAttachment.FileName;
            var updateResult = await _fileService.UploadDocumentAsync(chatAttachment.Attachment, update: true);
            chatAttachment.AttachmentUrl = updateResult.AttachmentUrl;

            var updateAttachmentResult = await _chatService.UpdateAttachment(id, chatAttachment);

            if (updateAttachmentResult.IsFailure)
            {
                return this.FromError(updateAttachmentResult.Error);
            }

            return Ok();
        }
    }
}
