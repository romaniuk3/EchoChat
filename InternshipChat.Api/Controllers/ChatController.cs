using AutoMapper;
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
        private readonly IChatService _chatService;
        private readonly IMapper _mapper;

        public ChatController(ILogger<ChatController> logger, IChatService chatService, IMapper mapper)
        {
            _logger = logger;
            _chatService = chatService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("create")]
        public IActionResult CreateChat([FromBody] CreateChatDTO chat)
        {
            _chatService.CreateChat(chat);

            return Ok();
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAll()
        {
            var chatInfoViews = await _chatService.GetAllChatsAsync();
            //var chats = _mapper.Map<ChatInfoDTO>(chatInfoViews);

            return Ok(chatInfoViews);
        }

        [HttpGet]
        [Route("user/{userId}")]
        public async Task<IActionResult> GetAllUserChats(int userId)
        {
            var userChats = await _chatService.GetUserChatsAsync(userId);
            if (userChats == null)
            {
                return BadRequest("User doesn't have any chats.");
            }
            return Ok(userChats);
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