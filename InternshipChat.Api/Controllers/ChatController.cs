using InternshipChat.Api.Hubs;
using InternshipChat.BLL.Services.Contracts;
using InternshipChat.DAL.Entities;
using InternshipChat.DAL.UnitOfWork;
using InternshipChat.Shared.DTO.ChatDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace InternshipChat.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly ILogger<ChatController> _logger;
        private readonly IMessageService _messageService;
        private readonly IChatService _chatService;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatController(ILogger<ChatController> logger, IMessageService messageService, IChatService chatService, IHubContext<ChatHub> hubContext)
        {
            _logger = logger;
            _messageService = messageService;
            _chatService = chatService;
            _hubContext = hubContext;
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
    }
}