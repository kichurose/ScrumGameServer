using System.Collections.Concurrent;
using ScrumGame.Models;

namespace ScrumGame.DataStore
{
    public class UserInfoContext : IUserInfoContext
    {
        public UserInfoContext() { }

        private ConcurrentDictionary<string, ConcurrentBag<UserInfo>> userData = new();

        public void Saveuser(UserInfo userInfo)
        {
            var usersInRoom = userData.GetOrAdd(userInfo.RoomId, _ => new ConcurrentBag<UserInfo>());
            usersInRoom.Add(userInfo);
        }

        public IEnumerable<UserInfo> GetAllUsersInRoom(string roomId) 
        {
            userData.TryGetValue(roomId, out var result);
            return result ?? Enumerable.Empty<UserInfo>();

        }
    }
}
