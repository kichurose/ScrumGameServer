using System.Security.Cryptography;
using ScrumGame.DataStore;
using ScrumGame.Models;
using ScrumGame.Services.Interfaces;
using RoomInfoContract = ScrumGame.Contracts;

namespace ScrumGame.Services
{
    public sealed class RoomsService : IRoomsService
    {
        private readonly IRoomInfoContext _roomInfoContext;

        public RoomsService(IRoomInfoContext roomInfoContext) {
            _roomInfoContext = roomInfoContext;
        }

        public string GenerateRoomId()
        {
            using var rng = RandomNumberGenerator.Create();
            byte[] bytes = new byte[8];
            rng.GetBytes(bytes);

            string result = "";
            foreach (byte b in bytes)
            {
                result += (b % 10).ToString();
            }

            return result;
        }


        public RoomInfo CreateRoom(RoomInfoContract.RoomInfo roomInfo)
        {
            RoomInfo result = new RoomInfo()
            {
                RoomId = GenerateRoomId(),
                RoomName = roomInfo.RoomName,
            };
            
            _roomInfoContext.AddRoom(result);
            return result;
        }

        public RoomInfo GetRoom(string roomId)
        {
            var result = _roomInfoContext.GetRoom(roomId);
            return result;
        }

    }
}
