using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ScrumGame.Services.Interfaces;

namespace ScrumGame.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public IActionResult SaveUser([FromBody] Contracts.UserInfo userInfo)
        {
            var userDetails = new Models.UserInfo()
            {
                RoomId = userInfo.RoomId,
                Username = userInfo.Name
            };

            _userService.AddUser(userDetails);

            return Ok();
        }

        [HttpGet]
        [Route("{roomId}")]
        public IActionResult GetAllUserByRoom(string roomId)
        {
            var test = _userService.GetUserInfoByRoomId(roomId);
            return Ok(test);
        }
    }
}
