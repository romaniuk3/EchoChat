using InternshipChat.BLL.Services.Contracts;
using InternshipChat.DAL.Entities;
using InternshipChat.DAL.UnitOfWork;
using InternshipChat.Shared.DTO.ChatDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipChat.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly ILogger<ChatController> _logger;
        private readonly IMessageService _messageService;
        private readonly IChatService _chatService;

        public ChatController(ILogger<ChatController> logger, IMessageService messageService, IChatService chatService)
        {
            _logger = logger;
            _messageService = messageService;
            _chatService = chatService;
        }

        [HttpPost]
        [Route("create")]
        public IActionResult CreateChat([FromBody] ChatDTO chat)
        {
            _chatService.CreateChat(chat);

            return Ok();
        }

        [HttpGet]
        [Route("all")]
        public IActionResult GetAll()
        {
            var chats = _chatService.GetAllChats();
            return Ok(chats);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetChat(int id)
        {
            var chat = await _chatService.GetChatAsync(id);
            return Ok(chat);
        }


        [HttpGet]
        [Route("GetMessages")]
        public IActionResult GetMessages()
        {
            List<Message> messages = new List<Message>();

            /* var msg1 = new Message();
            msg1.Id = 1;
            msg1.Content = "First msg";

            var msg2 = new Message();
            msg2.Id = 1;
            msg2.Content = "Second msg";
            messages.Add(msg1);
            messages.Add(msg2);
            _messageService.SendMessage(msg1);*/

            return Ok(messages);
        }
    }
}