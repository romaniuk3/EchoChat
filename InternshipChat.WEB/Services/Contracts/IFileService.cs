using InternshipChat.Shared.DTO.ChatDtos;
using Microsoft.AspNetCore.Components.Forms;

namespace InternshipChat.WEB.Services.Contracts
{
    public interface IFileService
    {
        public Task<string> ToBase64(IBrowserFile imageFile);
        Task<string?> UploadImage(IBrowserFile file);
        Task<ChatAttachmentDTO?> UploadAttachment(CreateChatAttachmentDTO attachmentDto);
        Task<ChatAttachmentDTO?> UploadPdf(CreateChatAttachmentDTO attachmentDto, string base64);
        Task<string?> UpdateAttachment(ChatAttachmentDTO attachment, string base64);
        Task DeleteAttachment(int attachmentId);
    }
}
