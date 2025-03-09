using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ScrumGame.Contracts;
using ScrumGame.Models;
using ScrumGame.Services;
using ScrumGame.Services.Interfaces;

namespace ScrumGame.Controllers
{
    [Route("api/room")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomsService _roomsService;
        private readonly IConfiguration _config;
        private readonly IUserService _userService;

        public RoomsController(IRoomsService roomsService, IConfiguration config,
            IUserService userService)
        {
            _roomsService = roomsService;
            _config = config;
            _userService = userService;
        }

        [HttpPost]

        public IActionResult CreateScrumRoom([FromBody] Contracts.RoomInfo roomInfo)
        {
            if (roomInfo is null)
            {
                return BadRequest("Room Info cannot be null");
            }

            var result = _roomsService.CreateRoom(roomInfo);
            var userInfo = new Models.UserInfo()
            {
                RoomId = result.RoomId,
                Username = result.RoomName
            };

            _userService.AddUser(userInfo);
            var jwtToken = GenerateJwtToken("Admin", result.RoomId);
            return Ok(new { result, jwtToken });
        }

        [HttpGet]
        [Route("{roomId}")]
        public IActionResult GetScrumRoom(string roomId)
        {
            var result = _roomsService.GetRoom(roomId);
            var token = GenerateJwtToken("User", result.RoomId);
            return Ok(new { token });
        }

        private string GenerateJwtToken(string role, string roomId)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim("userRole", role),
            new Claim("roomId", roomId)
        };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
