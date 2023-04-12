using InternshipChat.BLL.Services.Contracts;
using InternshipChat.DAL.Entities;
using InternshipChat.DAL.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace InternshipChat.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly ILogger<ChatController> _logger;
        private readonly IMessageService _messageService;

        public ChatController(ILogger<ChatController> logger, IMessageService messageService)
        {
            _logger = logger;
            _messageService = messageService;
        }

        [HttpGet]
        [Route("GetMessages")]
        public IActionResult GetMessages()
        {
            List<Message> messages = new List<Message>();

            var msg1 = new Message();
            msg1.Id = 1;
            msg1.Content = "First msg";

            var msg2 = new Message();
            msg2.Id = 1;
            msg2.Content = "Second msg";
            messages.Add(msg1);
            messages.Add(msg2);

            return Ok(messages);
        }
    }
}