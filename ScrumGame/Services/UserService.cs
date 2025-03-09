using ScrumGame.DataStore;
using ScrumGame.Models;
using ScrumGame.Services.Interfaces;

namespace ScrumGame.Services
{
    public class UserService : IUserService
    {
        private readonly IUserInfoContext _userInfoContext;

        public UserService(IUserInfoContext userInfoContext) { 
        _userInfoContext = userInfoContext;
        }

        public void AddUser(UserInfo userInfo)
        {
            _userInfoContext.Saveuser(userInfo);
        }

        public IEnumerable<UserInfo> GetUserInfoByRoomId(string roomId)
        {
            return _userInfoContext.GetAllUsersInRoom(roomId);
        }
    }
}
