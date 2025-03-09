using ScrumGame.Models;

namespace ScrumGame.DataStore
{
    public interface IUserInfoContext
    {
        void Saveuser(UserInfo userInfo);

        IEnumerable<UserInfo> GetAllUsersInRoom(string roomId);
    }
}
