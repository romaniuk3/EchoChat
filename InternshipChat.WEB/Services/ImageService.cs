using Blazored.LocalStorage;
using InternshipChat.DAL.Entities;
using InternshipChat.Shared.DTO.ChatDtos;
using InternshipChat.WEB.Services.Base;
using InternshipChat.WEB.Services.Contracts;
using Microsoft.AspNetCore.Components.Forms;
using System.Net;
using System.Text.Json;

namespace InternshipChat.WEB.Services
{
    public class ImageService : BaseHttpService, IImageService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ImageService(HttpClient httpClient, ILocalStorageService localStorage, IConfiguration configuration) : base(httpClient, localStorage)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

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

        public async Task<string?> Upload(IBrowserFile file)
        {
            var formData = new MultipartFormDataContent
            {
                { new StreamContent(file.OpenReadStream()), "file", file.Name }
            };

            var response = await _httpClient.PostAsync($"api/file/upload", formData);

            if (response.IsSuccessStatusCode)
            {
                var imgUrl = await response.Content.ReadAsStringAsync();
                return imgUrl;
            }

            return null;
        }

        public async Task<ChatAttachment?> UploadAttachment(ChatAttachmentDTO attachmentDto)
        {
            await GetBearerToken();

            var formData = new MultipartFormDataContent
            {
                { new StringContent(attachmentDto.SenderId.ToString()), "SenderId" },
                { new StringContent(attachmentDto.ChatId.ToString()), "ChatId" },
                { new StringContent(attachmentDto.FileName.ToString()), "FileName" },
                { new StreamContent(attachmentDto.Document.OpenReadStream()), "file", attachmentDto.Document.Name }
            };
            var functionBaseUrl = _configuration["AzFunctionBase"];
            var response = await _httpClient.PostAsync($"{functionBaseUrl}api/AttachmentStarter", formData);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                var readyAttachment = await response.Content.ReadFromJsonAsync<ChatAttachment>();
                Console.WriteLine("SUCCEEDED IN SERVICE");
                Console.WriteLine("EXTRACTED TEXT AFTER FUNCTION COMPLETED: " + readyAttachment.FileText);
                Console.WriteLine("ATTACHMENT ID: " + readyAttachment.Id);
                Console.WriteLine("ATTACHMENT CHATID: " + readyAttachment.ChatId);
                Console.WriteLine("ATTACHMENT USERID: " + readyAttachment.SenderId);

                return readyAttachment;
            } else if (response.StatusCode == HttpStatusCode.Accepted)
            {
                Console.WriteLine("In progress");
            }

            return null;
        }

        public async Task<string?> UploadPdf(string base64)
        {
            await GetBearerToken();
           
            var byteArray = Convert.FromBase64String(base64);
            var formData = new MultipartFormDataContent
            {
                { new StreamContent(new MemoryStream(byteArray)), "file", "filename.pdf" }
            };

            var response = await _httpClient.PostAsync($"api/file/upload/document", formData);

            if (response.IsSuccessStatusCode)
            {
                var pdfUrl = await response.Content.ReadAsStringAsync();
                return pdfUrl;
            }

            return null;
        }
    }
}
