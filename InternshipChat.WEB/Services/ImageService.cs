using InternshipChat.WEB.Services.Contracts;
using Microsoft.AspNetCore.Components.Forms;

namespace InternshipChat.WEB.Services
{
    public class ImageService : IImageService
    {
        public async Task<string> ToBase64(IBrowserFile file)
        {
            byte[] fileBytes;
            using (var ms = new MemoryStream())
            {
                await file.OpenReadStream().CopyToAsync(ms);
                fileBytes = ms.ToArray();
            }
            var contentType = file.ContentType;
            
            var base64String = Convert.ToBase64String(fileBytes);
            string dataUri = $"data:{contentType};base64,{base64String}";

            return dataUri;
        }
    }
}
