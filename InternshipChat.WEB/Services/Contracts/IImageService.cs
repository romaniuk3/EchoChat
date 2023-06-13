using InternshipChat.DAL.Entities;
using InternshipChat.Shared.DTO.ChatDtos;
using Microsoft.AspNetCore.Components.Forms;

namespace InternshipChat.WEB.Services.Contracts
{
    public interface IImageService
    {
        public Task<string> ToBase64(IBrowserFile imageFile);
        Task<string?> Upload(IBrowserFile file);
        Task<ChatAttachment?> UploadAttachment(ChatAttachmentDTO attachmentDto);
        Task<ChatAttachment?> UploadPdf(ChatAttachmentDTO attachmentDto, string base64);
        Task<string?> UpdateAttachment(ChatAttachment attachment, string base64);
        Task DeleteAttachment(int attachmentId);
    }
}
