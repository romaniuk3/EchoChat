using AutoMapper;
using InternshipChat.BLL.Services.Contracts;
using InternshipChat.DAL.Entities;
using InternshipChat.Shared.DTO.ChatDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InternshipChat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IMessageService _messageService;

        public MessageController(IUserService userService, IMapper mapper, IMessageService messageService)
        {
            _userService = userService;
            _mapper = mapper;
            _messageService = messageService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SendMessage(MessageDTO messageDto)
        {
            var userName = HttpContext.User.Identity!.Name;
            var user = await _userService.GetUserByNameAsync(userName);

            var message = _mapper.Map<Message>(messageDto);
            var res = _messageService.SendMessage(message, user.Id);

            return Ok(res);
        }
    }
}
