using ScrumGame.Models;

namespace ScrumGame.DataStore
{
    public interface IRoomInfoContext
    {
        void AddRoom(RoomInfo room);
        RoomInfo? GetRoom(string roomId);
        List<RoomInfo> GetAllRooms();
        bool RemoveRoom(string roomId);
    }

}


