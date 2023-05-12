using AutoMapper;
using FluentValidation;
using InternshipChat.Api.Extensions;
using InternshipChat.BLL.Services.Contracts;
using InternshipChat.DAL.Data;
using InternshipChat.DAL.Entities;
using InternshipChat.Shared.DTO;
using InternshipChat.Shared.DTO.UserDtos;
using InternshipChat.Shared.Models;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IValidator<LoginDto> _loginValidator;
        private readonly IValidator<RegisterUserDTO> _registerValidator;

        public AccountController(IAuthService authService, IValidator<LoginDto> loginValidator, IValidator<RegisterUserDTO> registerValidator)
        {
            _authService = authService;
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
        }

        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Register(RegisterUserDTO registerUserDTO)
        {
            var validation = await _registerValidator.ValidateAsync(registerUserDTO);
            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            var registerResult = await _authService.Register(registerUserDTO);

            if (!registerResult.Succeeded)
            {
                var errors = registerResult.Errors.Select(x => x.Description);

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
            var validation = await _loginValidator.ValidateAsync(loginDto);
            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            var authResponse = await _authService.Login(loginDto);
            if (!authResponse.Successful)
            {
                return BadRequest(authResponse);
            }

            return Ok(authResponse);
        }

        [Authorize]
        [HttpPost]
        [Route("change")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> ChangePassword(ChangePasswordModel model)
        {
            var changePasswordResult = await _authService.ChangePassword(model);

            if (changePasswordResult.IsFailure)
            {
                this.FromError(changePasswordResult.Error);
            }

            return Ok(changePasswordResult);
        }
    }
}
