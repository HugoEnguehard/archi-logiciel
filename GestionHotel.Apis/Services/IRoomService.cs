using GestionHotel.Apis.Models;

namespace GestionHotel.Apis.Services
{
    public interface IRoomService
    {
        Task<bool> AddRoom(Room room);
        Task<Room> GetRoomById(int id);
        Task<bool> GetRoomByDates(DateTime start_date, DateTime end_date);
        Task<Room> UpdateRoom(Room room);
        Task<bool> DeleteRoom(int id);
    }
}
