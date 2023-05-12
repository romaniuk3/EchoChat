using AutoMapper;
using FluentValidation;
using InternshipChat.Api.Extensions;
using InternshipChat.BLL.Services.Contracts;
using InternshipChat.BLL.Validators;
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
        private readonly IValidator<UpdateUserDTO> _updateValidator;

        public UsersController(IUserService userService, IMapper mapper, IValidator<UpdateUserDTO> updateValidator)
        {
            _userService = userService;
            _mapper = mapper;
            _updateValidator = updateValidator;
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
            var validation = await _updateValidator.ValidateAsync(updateUserDTO);
            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            var updateUserResult = await _userService.UpdateAsync(id, updateUserDTO);

            if (updateUserResult.IsFailure)
            {
                return this.FromError(updateUserResult.Error);
            }

            return Ok(_mapper.Map<UserDTO>(updateUserResult.Value));
        }
    }
}
