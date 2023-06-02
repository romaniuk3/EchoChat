using InternshipChat.Shared.DTO.ChatDtos;
using Microsoft.AspNetCore.Components.Forms;

namespace InternshipChat.WEB.Services.Contracts
{
    public interface IImageService
    {
        public Task<string> ToBase64(IBrowserFile imageFile);
        Task<string?> Upload(IBrowserFile file);
        Task<object> UploadAttachment(ChatAttachmentDTO attachmentDto);
    }
}
