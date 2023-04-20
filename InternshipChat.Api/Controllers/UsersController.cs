using InternshipChat.BLL.Services.Contracts;
using InternshipChat.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InternshipChat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("all")]
        public IActionResult GetAll([FromQuery] UserParameters userParameters)
        {
            var users = _userService.GetAll(userParameters);

            return Ok(users);
        }
    }
}
