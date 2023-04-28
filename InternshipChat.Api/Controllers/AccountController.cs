using AutoMapper;
using InternshipChat.BLL.Services.Contracts;
using InternshipChat.DAL.Data;
using InternshipChat.DAL.Entities;
using InternshipChat.Shared.DTO;
using InternshipChat.Shared.DTO.UserDtos;
using InternshipChat.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InternshipChat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Register(RegisterUserDTO registerUserDTO)
        {
            var possibleErrors = await _authService.Register(registerUserDTO);

            if (possibleErrors.Any())
            {
                var errors = possibleErrors.Select(x => x.Description);

                return BadRequest(new RegisterResult { Successful = false, Errors = errors });
            }

            return Ok(new RegisterResult { Successful = true });
        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            var authResponse = await _authService.Login(loginDto);

            if (!authResponse.Successful)
            {
                return BadRequest(authResponse);
            }

            return Ok(authResponse);
        }

        [HttpPost]
        [Route("change")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> ChangePassword(ChangePasswordModel model)
        {
            var changePasswordResult = await _authService.ChangePassword(model);

            if(!changePasswordResult.Successful)
            {
                return BadRequest(changePasswordResult);
            }

            return Ok(changePasswordResult);
        }
    }
}
