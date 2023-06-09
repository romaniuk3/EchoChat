using InternshipChat.BLL.Services.Contracts;
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
        public async Task<IActionResult> UploadDocument([FromForm] IFormFile file)
        {
            var blobUrl = await _fileService.UploadDocumentAsync(file);

            return Ok(blobUrl);
        }
    }
}
