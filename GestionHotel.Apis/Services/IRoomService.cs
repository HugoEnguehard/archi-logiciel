using GestionHotel.Apis.Models;

namespace GestionHotel.Apis.Services
{
    public interface IRoomService
    {
        Task<List<Room>?> GetRoomByDates(DateTime start_date, DateTime end_date);
        Task<Room?> GetRoomById(int id);
        Task<List<Room>?> GetDisponibleRoomsByDates(DateTime start_date, DateTime end_date);
        Task<List<Room>?> GetRoomsToClean();
        Task<bool> UpdateRoom(Room room);
        Task<bool> DeleteRoom(int id);
        Task<bool> AddRoom(Room room);
    }
}
