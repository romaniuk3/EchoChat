using Microsoft.AspNetCore.Components.Forms;

namespace InternshipChat.WEB.Services.Contracts
{
    public interface IImageService
    {
        public Task<string> ToBase64(IBrowserFile imageFile);
    }
}
