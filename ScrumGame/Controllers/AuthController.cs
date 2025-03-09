using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScrumGame.Services.Interfaces;

namespace ScrumGame.Controllers
{
    [Route("api/authenticate")]
    [ApiController]
    public class AuthController : ControllerBase
    {
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
    }
}
