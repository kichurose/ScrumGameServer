using System.Collections.Concurrent;
using ScrumGame.Models;

namespace ScrumGame.DataStore
{
    public class RoomInfoContext : IRoomInfoContext
    {
        private ConcurrentDictionary<string, RoomInfo> _rooms = new ConcurrentDictionary<string, RoomInfo>();

        public void AddRoom(RoomInfo room)
        {
            _rooms[room.RoomId] = room;
            Console.WriteLine(room.RoomId);
        }

        public RoomInfo? GetRoom(string roomId)
        {
            _rooms.TryGetValue(roomId, out var room);
            return room;
        }

        public List<RoomInfo> GetAllRooms()
        {
            return [.. _rooms.Values];
        }

        public bool RemoveRoom(string roomId)
        {
            return _rooms.TryRemove(roomId, out _);
        }
    }
}
