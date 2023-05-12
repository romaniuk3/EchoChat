using AutoMapper;
using FluentValidation;
using InternshipChat.BLL.Errors;
using InternshipChat.BLL.ServiceResult;
using InternshipChat.BLL.Services.Contracts;
using InternshipChat.DAL.Data;
using InternshipChat.DAL.Entities;
using InternshipChat.Shared.DTO;
using InternshipChat.Shared.DTO.UserDtos;
using InternshipChat.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IValidator<LoginDto> _loginValidator;
        private readonly IValidator<RegisterUserDTO> _registerValidator;

        public AuthService(IMapper mapper, UserManager<User> userManager, IConfiguration configuration, 
            IValidator<LoginDto> loginValidator, IValidator<RegisterUserDTO> registerValidator)
        {
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
        }

        public async Task<Result<LoginResult>> Login(LoginDto loginDto)
        {
            var validation = await _loginValidator.ValidateAsync(loginDto);
            if (!validation.IsValid)
            {
                return Result.Failure<LoginResult>(DomainErrors.Validation.ValidationError(validation.Errors));
            }

            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            bool isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            
            if(user == null || !isPasswordValid)
            {
                return Result.Failure<LoginResult>(DomainErrors.Auth.IncorrectData);
            }

            var token = await GenerateToken(user);

            return new LoginResult
            {
                Token = token,
                Successful = true
            };
        }

        public async Task<Result> ChangePassword(ChangePasswordModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id.ToString());

            if (user == null)
            {
                return Result.Failure(DomainErrors.User.NotFound);
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                return Result.Failure(DomainErrors.User.IncorrectPassword);
            }

            return Result.Success();
        }

        public async Task<Result> Register(RegisterUserDTO registerUserDTO)
        {
            var validation = await _registerValidator.ValidateAsync(registerUserDTO);
            if (!validation.IsValid)
            {
                return Result.Failure(DomainErrors.Validation.ValidationError(validation.Errors));
            }

            var user = _mapper.Map<User>(registerUserDTO);
            user.UserName = registerUserDTO.Email;

            await _userManager.CreateAsync(user, registerUserDTO.Password);

            return Result.Success();
        }

        private async Task<string> GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("id", user.Id.ToString(), ClaimValueTypes.Integer)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"])),
                signingCredentials: credentials                
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
