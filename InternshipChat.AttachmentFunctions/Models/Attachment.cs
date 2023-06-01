using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.AttachmentFunctions.Models
{
    public class Attachment
    {
        public string FileName { get; set; }
        public byte[] Content { get; set; }
        /*public Attachment(IFormFile formFile)
        {
            FileName= formFile.FileName;
            Content = ReadFileContent(formFile);
        }*/

        public static byte[] ReadFileContent(IFormFile formFile)
        {
            using var memoryStream = new MemoryStream();
            formFile.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
