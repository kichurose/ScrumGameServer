using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ScrumGame.Contracts;
using ScrumGame.Services;
using ScrumGame.Services.Interfaces;
using LoginRequest = ScrumGame.Contracts.LoginRequest;

namespace ScrumGame.Controllers
{
    [Route("api/authenticate")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IRoomsService _roomsService;
        private readonly IUserService _userService;
        private readonly IConfiguration _config;

        public AuthController(IUserService userService, IConfiguration configuration, IRoomsService roomsService)
        {
            _userService = userService;
            _config = configuration;
            _roomsService = roomsService;
        }

        [Authorize] // Requires JWT Token
        [HttpGet]
        [Route("{roomId}")]
        public IActionResult ValidateUserRole(string roomId)
        {
            var userClaims = HttpContext.User.Claims;
            // if key is role, then use ClaimTypes.Role
            var userRole = userClaims.FirstOrDefault(c => c.Type == "userRole")?.Value;
            var userRoomId = userClaims.FirstOrDefault(c => c.Type == "roomId")?.Value;

            if (string.IsNullOrEmpty(userRoomId) || userRoomId != roomId)
            {
                return Forbid("You are not authorized to join this room.");
            }

            return Ok(new { role = userRole });
        }

        //[Authorize]
        //[HttpPost]
        //public IActionResult Login(LoginRequest loginRequest)
        //{
        //    // We will create a room for login user
        //    var result = _roomsService.CreateRoom(new Contracts.RoomInfo { RoomName = loginRequest.Name});
        //    var userInfo = new Models.UserInfo()
        //    {
        //        RoomId = result.RoomId,
        //        Username = result.RoomName,
        //        Password = loginRequest.Password
        //    };

        //    _userService.AddUser(userInfo);
        //    return Ok();
        //}

        [HttpPost]
        public IActionResult SignUp(SignUpRequest signupRequest)
        {
            var result = _roomsService.CreateRoom(new Contracts.RoomInfo { RoomName = signupRequest.Name });

            // hash password
            var userInfo = new Models.UserInfo()
            {
                RoomId = result.RoomId,
                Username = signupRequest.Name,
                Password = signupRequest.Password
            };

            _userService.AddUser(userInfo);

            var token = GenerateJwtToken(signupRequest.Name, "Admin", result.RoomId);

            return Ok(new { roomId = userInfo.RoomId, token});
        }

        private string GenerateJwtToken(string username, string role, string roomId)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("username", username),
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
