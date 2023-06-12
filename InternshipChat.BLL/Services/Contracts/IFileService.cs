using InternshipChat.Shared.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.BLL.Services.Contracts
{
    public interface IFileService
    {
        string GetUniqueFileName(string fileName);
        Task<string> UploadImageAsync(IFormFile file);
        Task<UploadAttachmentResult> UploadDocumentAsync(FileModel fileModel, bool update = false);
        string GenerateSasTokenForBlobContainer();
        string GenerateSasTokenForBlob(string blobName, string? containerName = null);
    }
}
