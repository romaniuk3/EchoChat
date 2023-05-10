using AutoMapper;
using InternshipChat.Api.Extensions;
using InternshipChat.BLL.Services.Contracts;
using InternshipChat.DAL.Entities;
using InternshipChat.Shared.DTO;
using InternshipChat.Shared.DTO.UserDtos;
using InternshipChat.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InternshipChat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<PagingResponseDTO<UserDTO>>> GetAll([FromQuery] UserParameters userParameters)
        {
            var users = await _userService.GetAllAsync(userParameters);
            var mappedUsers = _mapper.Map<PagingResponseDTO<UserDTO>>(users);

            return Ok(mappedUsers);
        }

        [HttpGet("{id}")]
        public ActionResult<UserDTO> GetUser(int id)
        {
            var userResult = _userService.GetUser(id);

            if (userResult.IsFailure)
            {
                return this.FromError(userResult.Error);
            }

            return Ok(_mapper.Map<UserDTO>(userResult.Value));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserDTO>> UpdateUser(int id, [FromBody] UpdateUserDTO updateUserDTO)
        {
            var user = _userService.GetUser(id);

            if (user == null)
            {
                return NotFound();
            }

            var updatedUser = await _userService.UpdateAsync(id, updateUserDTO);

            return Ok(_mapper.Map<UserDTO>(updatedUser));
        }
    }
}
