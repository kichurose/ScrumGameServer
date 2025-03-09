using ScrumGame.Models;

namespace ScrumGame.Services.Interfaces
{
    public interface IUserService
    {

        void AddUser(UserInfo userInfo);

        IEnumerable<UserInfo> GetUserInfoByRoomId(string roomId);
    }
}
