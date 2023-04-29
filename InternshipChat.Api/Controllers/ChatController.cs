using AutoMapper;
using InternshipChat.Api.Extensions;
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
        public async Task<IActionResult> CreateChat([FromBody] CreateChatDTO chat)
        {
            var createResult = await _chatService.CreateChatAsync(chat);
            if (createResult.IsFailure)
            {
                return this.FromError(createResult.Error);
            }

            return Ok();
        }

        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<ChatInfoView>> GetAll()
        {
            var chatInfoViews = await _chatService.GetAllChatsAsync();
            //var chats = _mapper.Map<ChatInfoDTO>(chatInfoViews);

            return Ok(chatInfoViews);
        }

        [HttpGet]
        [Route("user/{userId}")]
        public async Task<ActionResult<ChatDTO>> GetAllUserChats(int userId)
        {
            var userChatsResult = await _chatService.GetUserChatsAsync(userId);
            if (userChatsResult.IsFailure)
            {
                return this.FromError(userChatsResult.Error);
            }

            var chatDtos = _mapper.Map<IEnumerable<ChatDTO>>(userChatsResult.Value);
            return Ok(chatDtos);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ChatDTO>> GetChat(int id)
        {
            var chatResult = await _chatService.GetChatAsync(id);
            if (chatResult.IsFailure)
            {
                return this.FromError(chatResult.Error);
            }

            var chatDto = _mapper.Map<ChatDTO>(chatResult.Value);
            return Ok(chatDto);
        }
    }
}