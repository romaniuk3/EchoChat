using AutoMapper;
using InternshipChat.BLL.Services.Contracts;
using InternshipChat.DAL.Entities;
using InternshipChat.Shared.DTO;
using InternshipChat.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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

        public AuthService(IMapper mapper, UserManager<User> userManager, IConfiguration configuration)
        {
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<LoginResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            bool isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            
            if(user == null || !isPasswordValid)
            {
                return new LoginResult
                {
                    Successful = false,
                    Error = "Username and password are invalid."
                };
            }

            var token = await GenerateToken(user);

            return new LoginResult
            {
                Token = token,
                Successful = true
            };
        }

        public async Task<ChangePasswordResult> ChangePassword(ChangePasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                return new ChangePasswordResult
                {
                    Successful = false,
                    Errors = result.Errors.Select(e => e.Description)
                };
            }

            return new ChangePasswordResult
            {
                Successful = true
            };
        }

        public async Task<IEnumerable<IdentityError>> Register(UserDTO userDto)
        {
            var user = _mapper.Map<User>(userDto);
            user.UserName = userDto.Email;

            var creatingResult = await _userManager.CreateAsync(user, userDto.Password);

            if (creatingResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
            }

            return creatingResult.Errors;
        }

        private async Task<string> GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var userClaims = await _userManager.GetClaimsAsync(user);
            //var roles = await _userManager.GetRolesAsync(user);

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
