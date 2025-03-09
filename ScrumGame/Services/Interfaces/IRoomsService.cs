using ScrumGame.Models;
using RoomInfoContract = ScrumGame.Contracts;

namespace ScrumGame.Services.Interfaces
{
    public interface IRoomsService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string GenerateRoomId();

        RoomInfo CreateRoom(RoomInfoContract.RoomInfo roomInfo);

        RoomInfo GetRoom(string roomId);
    }
}